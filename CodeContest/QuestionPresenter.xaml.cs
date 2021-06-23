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

        public QuestionPresenter()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
        }

        public void Present(Question question)
        {
            Clear();
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
            String word = "";
            TrieNode node = null;
            foreach (var ch in line.ToCharArray())
            {
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
                    word = "";
                    node = null;
                }
            }
            Run run = new Run();
            run.Text = line;
            run.FontSize = 20;
            run.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Consolas");
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(run);
            ContentBlock.Blocks.Add(paragraph);
        }
    }
}
