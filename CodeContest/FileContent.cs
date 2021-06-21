using System;
using System.Collections.Generic;

namespace CodeContest
{
    public struct FileContent
    {
        public FileContent(string title, IList<String> lines)
        {
            Title = title;
            Lines = lines;
        }
        public string Title { get; }
        public IList<string> Lines { get; }
        public int LineCounts { get => Lines.Count; }
    }
}
