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

        public void ChangeToReplaced()
        {
            int len = rawText.Length;
            displayedRun.Text = new string('_', len);
            displayedRun.FontSize = QuestionPresenter.ContentFontSize;
            displayedRun.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Consolas");
        }
    }
}
