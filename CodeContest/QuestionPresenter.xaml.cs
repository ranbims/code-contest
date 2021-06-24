using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using rm.Trie;
using Question = CodeContest.FileContent;
using System.Diagnostics;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeContest
{
    public sealed partial class QuestionPresenter : UserControl
    {
        private DispatcherTimer timer;
        private int totalTime = 10000; // milliseconds
        private int currentLine;
        private Question currentQuestion;

        private ISet<string> functions = new HashSet<string>();
        private ISet<string> variables = new HashSet<string>();
        private Trie trie = new Trie();

        private ISet<ReplacableFunction> functionCode = new HashSet<ReplacableFunction>();
        private ISet<ReplacableVariable> variableCode = new HashSet<ReplacableVariable>();

        private ReplacableCotentColorPalette palette = new ReplacableCotentColorPalette();

        public QuestionPresenter()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
        }

        public void Present(Question question)
        {
            Clear();
            palette.reset();
            currentQuestion = question;
            int lineCount = question.LineCounts;
            int interval = totalTime / lineCount;
            timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        public void Resume()
        {
            timer.Start();
        }

        public void Clear()
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
            ContentBlock.Blocks.Clear();
            currentLine = 0;
        }

        private void Timer_Tick(object sender, object e)
        {
            var count = currentQuestion.LineCounts;
            if (currentLine < count)
            {
                string line = currentQuestion.Lines[currentLine];
                if (line.StartsWith("#define"))
                {
                    string definedContent = line.Substring("#define".Length).Trim();
                    var contents = definedContent.Split('=');
                    var key = contents[0];
                    var value = contents[1].Split(',');
                    ISet<string> contentSet = null;
                    if (key.Equals("function"))
                    {
                        contentSet = functions;
                    }
                    else if (key.Equals("variable"))
                    {
                        contentSet = variables;
                    }

                    foreach (var valueEntry in value)
                    {
                        contentSet.Add(valueEntry);
                        trie.AddWord(valueEntry);
                    }
                }
                else
                {
                    // Add a new line
                    appendLine(line);
                }
                currentLine++;
            } 
            else
            {
                timer.Stop();
            }
        }

        private void appendLine(string line)
        {
            Paragraph paragraph = new Paragraph();

            String word = "";
            TrieNode node = null;
            var chars = line.ToCharArray();
            int index = 0; // where the to be added run start
            //foreach (var ch in line.ToCharArray())
            for (int i = 0; i < chars.Length; i++)
            {
                char ch = chars[i];
                if (word.Equals("")) {
                    node = trie.GetTrieNode(ch.ToString());
                    if (node != null)
                    {
                        word += node.Character;
                    }
                    else
                    {
                        word = "";
                        node = null;
                        continue;
                    }
                }
                else
                {
                    node = node.GetChild(ch);
                    if (node != null)
                    {
                        word += node.Character;
                    }
                    else
                    {
                        word = "";
                        node = null;
                        continue;
                    }
                }
                if (node.IsWord)
                {
                    Debug.WriteLine("find word: " + word);
                    
                    if (i + 1 - word.Length > index)
                    {
                        int wordStartIndex = i - word.Length + 1;
                        paragraph.Inlines.Add(constructRun(line.Substring(index, wordStartIndex - index)));
                        string replacedContent = line.Substring(wordStartIndex, word.Length);
                        if (functions.Contains(replacedContent))
                        {
                            var function = new ReplacableFunction(replacedContent);
                            paragraph.Inlines.Add(function.DisplayedRun);
                            functionCode.Add(function);
                        }
                        else if (variables.Contains(replacedContent))
                        {
                            var variable = new ReplacableVariable(replacedContent, palette.getColorByContent(replacedContent));
                            paragraph.Inlines.Add(variable.DisplayedRun);
                            variableCode.Add(variable);
                        }   
                    }
                    else
                    {
                        paragraph.Inlines.Add(constructRun(line.Substring(index, word.Length)));
                    }

                    word = "";
                    node = null;
                    index = i + 1;
                }
            }

            if (index < chars.Length)
            {
                paragraph.Inlines.Add(constructRun(line.Substring(index, line.Length - index)));
            }

            //run run = new run();
            //run.text = line;
            //run.fontsize = 20;
            //run.fontfamily = new windows.ui.xaml.media.fontfamily("consolas");
            //paragraph paragraph = new paragraph();
            //paragraph.inlines.add(run);

            //run run2 = new run();
            //run2.text = line;
            //run2.fontsize = 10;
            //run2.fontfamily = new windows.ui.xaml.media.fontfamily("consolas");
            //paragraph.inlines.add(run2);
            ContentBlock.Blocks.Add(paragraph);
        }

        public void ShowTip_1()
        {
            foreach (var replacable in variableCode)
            {
                replacable.ChangeToRaw();
            }
        }

        public void ShowTip_2()
        {
            foreach (var replacable in functionCode)
            {
                replacable.ChangeToRaw();
            }
        }

        private Run constructRun(string text)
        {
            Run run = new Run();
            run.Text = text;
            run.FontSize = 20;
            run.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Consolas");
            return run;
        }
    }
}
