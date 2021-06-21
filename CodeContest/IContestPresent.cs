namespace CodeContest
{
    public interface IContestPresent
    {
        void PlayQuestion();
        void PlayQuestionWithStrategy(PresentStrategy stratery);
        void SelectNext();
        void SelectPrevious();
        void ShowAnswer();
    }
}
