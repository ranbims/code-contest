using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Question = CodeContest.FileContent;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeContest
{
    public sealed partial class QuestionPresenter : UserControl
    {
        private DispatcherTimer timer;
        private int totalTime = 60000; // milliseconds
        private int currentLine;
        private Question currentQuestion;

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
                // Add a new line
                appendLine(currentQuestion.Lines[currentLine]);
                currentLine++;
            } 
            else
            {
                timer.Stop();
            }
        }

        private void appendLine(string line)
        {
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
