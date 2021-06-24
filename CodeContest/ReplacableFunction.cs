using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace CodeContest
{
    public class ReplacableFunction : IReplacableRun
    {
        private string rawText;
        private Run displayedRun;

        public ReplacableFunction(string functionName)
        {
            rawText = functionName;
            displayedRun = new Run();
            ChangeToReplaced();
        }

        public string RawText => rawText;

        public Run DisplayedRun => displayedRun;

        public void ChangeToRaw()
        {
            displayedRun.Text = rawText;
        }

        public void ChangeToPartialReplaced()
        {
            Random random = new Random();
            int first = random.Next(0, rawText.Length - 1);
            int second = rawText.Length > 5 ? random.Next(0, rawText.Length - 1) : -1;
            var newChars = new Char[rawText.Length];
            for (int i = 0; i < rawText.Length; i++)
            {
                if (i == first || i == second)
                {
                    newChars[i] = rawText[i];
                }
                else
                {
                    newChars[i] = '_';
                }
            }
            displayedRun.Text = new string(newChars);
        }

        public void ChangeToReplaced()
        {
            int len = rawText.Length;
            displayedRun.Text = new string('_', len);
            displayedRun.FontSize = QuestionPresenter.ContentFontSize;
            displayedRun.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Consolas");
        }
    }
}
