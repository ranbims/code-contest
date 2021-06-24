using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Contest contest;
        private int currentIndex;
        private PresentStrategy currentStrategy = PresentStrategy.Sequence;
        private ObservableCollection<string> questions = new ObservableCollection<string>();

        public bool IsSettingPanelVisiblie = false;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            contest = await Contest.ContestLoader.LoadContestAsync();
            for (int i = 0; i < contest.QuestionCount; i++)
            {
                int index = i + 1;
                questions.Add($"{index}. {new String('*', contest.Questions[i].Title.Length)}");
            }

            currentIndex = 0;

            Debug.WriteLine(contest.Questions[0].LineCounts);

            PlayQuestion();

            PreviousButton.Click += (s, ev) => SelectPrevious();
            NextButton.Click += (s, ev) => SelectNext();
            PauseButton.Click += (s, ev) => QuestionPresenter.Pause();
            ResumeButton.Click += (s, ev) => QuestionPresenter.Resume();
            ReplayButton.Click += (s, ev) => PlayQuestion();
            ShowTip1Button.Click += (s, ev) => QuestionPresenter.ShowTip_1();
            ShowTip2Button.Click += (s, ev) => QuestionPresenter.ShowTip_2();
            QuestionListButton.Click += (s, ev) => QuestionList.Visibility = QuestionList.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            SettingButton.Click += (s, ev) => SettingsPanel.Visibility = SettingsPanel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            TextSizeConfirmButton.Click += (s, ev) =>
            {
                try
                {
                    QuestionPresenter.ContentFontSize = int.Parse(TextSizeInput.Text);
                }
                catch (Exception)
                {

                }
                
            };
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

        private void QuestionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var splits = (e.ClickedItem as string).Split('.');
            currentIndex = int.Parse(splits[0]) - 1;
            PlayQuestion();
        }
    }
}
