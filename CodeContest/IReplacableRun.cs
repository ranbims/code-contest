using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace CodeContest
{
    public interface IReplacableRun
    {
        string RawText { get; }
        Run DisplayedRun { get; }
        void ChangeToReplaced();
        void ChangeToPartialReplaced();
        void ChangeToRaw();
    }
}
