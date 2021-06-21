using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace CodeContest
{
    public class Contest
    {
        private Contest(IList<FileContent> questions)
        {
            Questions = questions;
        }
        public IList<FileContent> Questions { get; }
        public int QuestionCount { get => Questions.Count; }

        public static class ContestLoader
        {
            public static async Task<Contest> LoadContestAsync()
            {
                var installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var contestFolder = await installFolder.GetFolderAsync(@"Assets\Contest");
                var questionFiles = await contestFolder.GetFilesAsync();
                var questions = new List<FileContent>();
                foreach (var file in questionFiles)
                {
                    var lines = await FileIO.ReadLinesAsync(file);
                    FileContent fileContent = new FileContent(file.Name, lines);
                    questions.Add(fileContent);
                }

                return new Contest(questions);
            }
        }
    }
}
