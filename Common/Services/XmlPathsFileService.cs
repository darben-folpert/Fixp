using System;
using System.IO;
using Common.Entities;

namespace Common.Services
{
    public class XmlPathsFileService
    {
        public XmlPathsFile LoadXmlPathsFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                var errorMessage = string.Format("Error while reading the xml paths file: File not found");
                errorMessage = string.Format("{0}\nFile name specified:\n{1}", errorMessage, fileName);
                throw new Exception(errorMessage);
            }

            var xmlPathFile = new XmlPathsFile();
            var listOfRawPaths = File.ReadAllLines(fileName);

            foreach (var oneRawPath in listOfRawPaths)
            {
                if (oneRawPath.Length > 1)
                    xmlPathFile.AddXmlPath(oneRawPath);
            }
            return xmlPathFile;
        }
    }
}
