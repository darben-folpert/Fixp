using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common.Services;
using Common.Entities;

namespace FindXmlPathsServices.Services
{
    public class FindMissingsInFileService
    {
        string _errMsg;
        XmlPathsFileService xmlPathsFileService;
        XmlFileService xmlFileService;

        public FindMissingsInFileService(XmlPathsFileService xmlPathsFileService, XmlFileService xmlFileService)
        {
            _errMsg = string.Empty;
            this.xmlPathsFileService = xmlPathsFileService;
            this.xmlFileService = xmlFileService;
        }

        public string ErrorMessage
        {
            get { return _errMsg; }
        }

        public bool AllInputsAreFilled(string xmlPathsFile, string xmlFileToCheck, string outputFolder)
        {
            string listOfMissings = string.Empty;

            if (string.IsNullOrEmpty(xmlPathsFile))
                listOfMissings = string.Format("{0}\n- Xml paths file", listOfMissings);

            if (string.IsNullOrEmpty(xmlFileToCheck))
                listOfMissings = string.Format("{0}\n- Xml file to check", listOfMissings);

            if (string.IsNullOrEmpty(outputFolder))
                listOfMissings = string.Format("{0}\n- Output folder", listOfMissings);

            if (!string.IsNullOrEmpty(listOfMissings))
            {
                _errMsg = string.Format("Please fill in the missing inputs:{0}", listOfMissings);
                return false;
            }

            return true;
        }

        public bool AllInputsAreValid(string xmlPathsFile, string xmlFileToCheck, string outputFolder)
        {
            string listOfMissings = string.Empty;

            if (!File.Exists(xmlPathsFile))
                listOfMissings = string.Format("{0}\n- Xml paths file : specified file doesn't exist !", listOfMissings);

            if (!File.Exists(xmlFileToCheck))
                listOfMissings = string.Format("{0}\n- Xml file to check : specified file doesn't exist !", listOfMissings);

            if (!Directory.Exists(outputFolder))
                listOfMissings = string.Format("{0}\n- Output folder : specified folder doesn't exist !", listOfMissings);

            if (!string.IsNullOrEmpty(listOfMissings))
            {
                _errMsg = string.Format("Please fix the followings:{0}", listOfMissings);
                return false;
            }

            return true;
        }

        void GenerateListOfMissingsInFile(string xmlPathsFileName, string xmlFileToCheck, string outputFolder)
        {
            // fill in missing code here
            // STEPS:
            // 1) Find dependencies to FileContentSeeker
            //    -> probably the class that loads xml docs (LightXmlFileLoader)
            // 2) Convert the class LightXmlFileLoader into some kind of service in Common.Services
            //    -> refactor

            // retrieve list of missings here, then write that list to output files in specified output folder
            var listOfMissings = GetListOfMissingsInFile(xmlPathsFileName, xmlFileToCheck);

            // use listOfMissings in order to
            // -> generate output file in destination folder: outputFolder
        }

        public IEnumerable<string> GetListOfMissingsInFile(string xmlPathsFileName, string xmlFileToCheck)
        {
            var listOfMissings = new List<string>();
            try
            {
                var xmlPathsFile = xmlPathsFileService.LoadXmlPathsFile(xmlPathsFileName);
                foreach (var xmlPath in xmlPathsFile.XmlPaths)
                {
                    var rawPath = FormatPathFromListToRaw(xmlPath);
                    if (!XmlFileContainsElement(xmlFileToCheck, xmlPath))
                        listOfMissings.Add(rawPath);
                }
                return listOfMissings;

            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return null;
        }

        bool XmlFileContainsElement(string fileName, IEnumerable<string> xmlPath)
        {
            if (!File.Exists(fileName))
            {
                var errorMessage = "The specified file doesn't exist !";
                errorMessage = string.Format("{0}\nFile not found: {1}", errorMessage, fileName);
                throw new FileNotFoundException(errorMessage);
            }

            var loadedXml = xmlFileService.LoadXmlFile(fileName);

            var xmlPathFound = FindPathThroughChildren
                (loadedXml.RootNode, loadedXml.RootNode, xmlPath);

            return xmlPathFound;
        }

        bool FindPathThroughChildren(XmlNodeEntity parentNode, XmlNodeEntity rootNode, IEnumerable<string> xmlPathToFind)
        {
            foreach (var childNode in parentNode.ChildrenNodes)
            {
                var xmlPath = BuildXmlPath(childNode, rootNode);
                if (ListsAreEquals(xmlPath, xmlPathToFind))
                    return true;

                var xmlPathFound = FindPathThroughChildren(childNode, rootNode, xmlPathToFind);
                if (xmlPathFound)
                    return true;
            }
            return false;
        }

        List<string> BuildXmlPath(XmlNodeEntity deepestNode, XmlNodeEntity rootNode)
        {
            var path = new List<string>();
            var currentNode = deepestNode;

            while (!currentNode.Equals(rootNode))
            {
                path.Add(currentNode.Name);
                currentNode = currentNode.Parent;
            }

            path.Add(rootNode.Name);
            path.Reverse();
            return path;
        }

        bool ListsAreEquals(IEnumerable<string> listOne, IEnumerable<string> listTwo)
        {
            if (!listOne.Count().Equals(listTwo.Count()))
                return false;

            var idxInListTwo = 0;
            foreach (var eltFromListOne in listOne)
            {
                var eltFromListTwo = listTwo.ElementAt(idxInListTwo);
                if (!eltFromListOne.Equals(eltFromListTwo))
                    return false;
                idxInListTwo++;
            }

            return true;
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
