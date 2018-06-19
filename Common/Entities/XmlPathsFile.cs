using System.Linq;
using System.Collections.Generic;

namespace Common.Entities
{
    public class XmlPathsFile
    {
        readonly IEnumerable<IEnumerable<string>> xmlPaths;

        public XmlPathsFile()
        {
            xmlPaths = new List<IEnumerable<string>>();
        }

        public int Count
        {
            get { return xmlPaths.ToList().Count; }
        }

        public IEnumerable<IEnumerable<string>> XmlPaths
        {
            get { return xmlPaths; }
        }

        public void AddXmlPath(string rawXmlPath)
        {
            if (!XmlPathExists(rawXmlPath))
            {
                var formattedPath = FormatPathFromSingleStringToList(rawXmlPath);
                ((List<IEnumerable<string>>)xmlPaths).Add(formattedPath);
            }
        }

        bool XmlPathExists(string newXmlPath)
        {
            foreach (var xmlPath in xmlPaths)
            {
                var formattedPath = FormatPathFromListToRaw(xmlPath);
                if (formattedPath.Equals(newXmlPath))
                    return true;
            }
            return false;
        }

        List<string> FormatPathFromSingleStringToList(string rawPath)
        {
            return rawPath.Split('/').ToList();
        }

        string FormatPathFromListToRaw(IEnumerable<string> pathAsList)
        {
            var rawPath = "";
            bool first = true;

            foreach (var oneLevelInPath in pathAsList)
            {
                if (first)
                {
                    rawPath = oneLevelInPath;
                    first = false;
                }
                else
                    rawPath = string.Format("{0}/{1}", rawPath, oneLevelInPath);
            }
            return rawPath;
        }
    }
}
