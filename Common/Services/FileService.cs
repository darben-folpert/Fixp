using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common.Services
{
    public class FileService
    {
        public IEnumerable<string> GetFileList(string folderSource, bool includeSubFolders)
        {
            var directoryInformation = new DirectoryInfo(folderSource);
            return includeSubFolders ?
                directoryInformation.GetFiles("*.*", SearchOption.AllDirectories).Select(x => x.FullName) :
                directoryInformation.GetFiles("*.*", SearchOption.TopDirectoryOnly).Select(x => x.FullName);
        }

        public IEnumerable<string>
            GetFileList(string folderSource, bool includeSubFolders, string fileNameMatchingPattern)
        {
            var directoryInformation = new DirectoryInfo(folderSource);
            var result = includeSubFolders ?
                directoryInformation.GetFiles("*.*", SearchOption.AllDirectories).Select(x => x.FullName) :
                directoryInformation.GetFiles("*.*", SearchOption.TopDirectoryOnly).Select(x => x.FullName);

            return result
                .Where(x => Regex.Match(x, fileNameMatchingPattern).Success);
        }
    }
}
