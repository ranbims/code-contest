using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace CodeContest
{
    public class ReplacableVariable : IReplacableRun
    {
        private string rawText;
        private Run displayedRun;
        private SolidColorBrush textBrush;

        public ReplacableVariable(string functionName, Color textColor)
        {
            rawText = functionName;
            displayedRun = new Run();
            textBrush = new SolidColorBrush(textColor);
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
            displayedRun.Text = new string('*', len);
            displayedRun.Foreground = textBrush;
            displayedRun.FontSize = QuestionPresenter.ContentFontSize;
            displayedRun.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Consolas");
        }
    }
}
