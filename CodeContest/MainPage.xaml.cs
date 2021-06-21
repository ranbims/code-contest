using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CodeContest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IContestPresent
    {
        private DispatcherTimer testTimer;
        private Contest contest;
        private int currentIndex;
        private PresentStrategy currentStrategy = PresentStrategy.Sequence;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            contest = await Contest.ContestLoader.LoadContestAsync();
            currentIndex = 0;

            Debug.WriteLine(contest.Questions[0].LineCounts);

            testTimer = new DispatcherTimer();
            testTimer.Interval = new TimeSpan(0, 0, 1);
            testTimer.Tick += TestTimer_Tick;
            testTimer.Start();

            PlayQuestion();

            QuestionPresenter.SizeChanged += ContentBlock_SizeChanged;
        }

        private void ContentBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentScrollViewer.ChangeView(0, ContentScrollViewer.ScrollableHeight, 1);
        }

        public void PlayQuestion()
        {
            PlayQuestionWithStrategy(currentStrategy);
        }

        public void PlayQuestionWithStrategy(PresentStrategy stratery)
        {
            if (stratery == PresentStrategy.Sequence)
            {
                QuestionPresenter.Present(contest.Questions[currentIndex]);
            }
        }

        public void SelectNext()
        {
            int count = contest.QuestionCount;
            if (currentIndex < count - 1)
            {
                currentIndex += 1;
                PlayQuestion();
            }
        }

        public void SelectPrevious()
        {
            int count = contest.QuestionCount;
            if (currentIndex > 0)
            {
                currentIndex -= 1;
                PlayQuestion();
            }
        }

        public void ShowAnswer()
        {
            throw new NotImplementedException();
        }

        private void TestTimer_Tick(object sender, object e)
        {
            Paragraph paragraph = new Paragraph();
            Run run = new Run();
            run.Text = "Hello";
            paragraph.Inlines.Add(run);
            //ContentBlock.Blocks.Add(paragraph);
        }
    }
}
