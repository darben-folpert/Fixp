using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FindXmlPathsServices.Services;
using Common.Services;

namespace Test.FindXmlPathsServices.Services
{
    [TestClass]
    public class FindMissingsInFileServiceTest
    {
        readonly FindMissingsInFileService findMissingsService;
        readonly XmlPathsFileService xmlPathsFileService;
        readonly XmlFileService xmlFileService;

        public FindMissingsInFileServiceTest()
        {
            xmlPathsFileService = new XmlPathsFileService();
            xmlFileService = new XmlFileService();
            findMissingsService = new FindMissingsInFileService(xmlPathsFileService, xmlFileService);
        }

        [TestMethod]
        public void ShouldTestThatAllInputsAreFilled()
        {
            Assert.IsFalse(CheckIfAllInputsAreFilled("", "", ""));
            Assert.IsFalse(CheckIfAllInputsAreFilled("", "bbb", ""));
            Assert.IsFalse(CheckIfAllInputsAreFilled("", "bbb", "ccc"));
            Assert.IsFalse(CheckIfAllInputsAreFilled("aaa", "", ""));
            Assert.IsFalse(CheckIfAllInputsAreFilled("aaa", "bbb", ""));
            Assert.IsTrue(CheckIfAllInputsAreFilled("aaa", "bbb", "ccc"));
            Assert.IsFalse(CheckIfAllInputsAreFilled("", "", "ccc"));
            Assert.IsFalse(CheckIfAllInputsAreFilled("aaa", "", "ccc"));
        }

        [TestMethod]
        public void ShouldTestThatAllInputsAreValid()
        {
            var xmlPathsFile = @"Data\XmlPathsFile-sample01.txt";
            var xmlFileToCheck = @"Data\tmpFile.txt";
            Assert.IsTrue(CheckIfAllInputsAreValid(xmlPathsFile, xmlFileToCheck, "Data"));
            Assert.IsFalse(CheckIfAllInputsAreValid(xmlPathsFile, xmlFileToCheck, "notAfolder"));
            Assert.IsFalse(CheckIfAllInputsAreValid("notApathToAfile", xmlFileToCheck, "Data"));
            Assert.IsFalse(CheckIfAllInputsAreValid("blabla", "nothing", "notAfolder"));
        }

        [TestMethod]
        public void ShouldGetCorrectListOfMissingsInFile()
        {
            var expectedList = ArrangeListOfMissings();
            var xmlPathsFile = @"Data\XmlPathsFile-sample01.txt";
            var missingsInFile = findMissingsService.GetListOfMissingsInFile(xmlPathsFile, @"Data\sample01.xml");
            Assert.IsTrue(expectedList.All(missingsInFile.Contains));
        }

        bool CheckIfAllInputsAreFilled(string xmlPathsFile, string xmlFileToCheck, string outputFolder)
        {
            return findMissingsService.AllInputsAreFilled(xmlPathsFile, xmlFileToCheck, outputFolder);
        }

        bool CheckIfAllInputsAreValid(string xmlPathsFile, string xmlFileToCheck, string outputFolder)
        {
            return findMissingsService.AllInputsAreValid(xmlPathsFile, xmlFileToCheck, outputFolder);
        }

        IEnumerable<string> ArrangeListOfMissings()
        {
            return new List<string>()
            {
                "LegalEntityExtenedStructure/OfficialList/Official/NaturalPerson/AddressCommon/ForeignAddress/Bullshit",
                "LegalEntityStructure/OfficialList/Official/NaturalPerson/AddressCommon/ForeignAddress/Country",
                "LegalEntityExtenedStructure/OfficialList/Official/NaturalPerson/Nothing"
            };
        }
    }
}
