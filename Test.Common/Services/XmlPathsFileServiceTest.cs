using System.Linq;
using Common.Services;
using Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Common.Services
{
    [TestClass]
    public class XmlPathsFileServiceTest
    {
        readonly XmlPathsFileService xmlPathsFileService;
        readonly XmlPathsFile xmlPathsFile;
        const string xmlPathsFileName01 = @"Data\Services\XmlPaths\XmlPaths01.txt";

        public XmlPathsFileServiceTest()
        {
            xmlPathsFileService = new XmlPathsFileService();
            xmlPathsFile = xmlPathsFileService.LoadXmlPathsFile(xmlPathsFileName01);
        }

        [TestMethod]
        public void ShouldLoad3XmlPathsInFile()
        {
            Assert.AreEqual(3, xmlPathsFile.Count);
        }

        [TestMethod]
        public void ShouldLoadRightContentOfXmlPathsFile()
        {
            var firstPath = xmlPathsFile.XmlPaths.ToList()[0];
            Assert.AreEqual("PathLevelRoot-A", firstPath.ToList()[0]);
            Assert.AreEqual("PathLevel-01-A", firstPath.ToList()[1]);
            Assert.AreEqual("PathLevel-02-A", firstPath.ToList()[2]);

            var secondPath = xmlPathsFile.XmlPaths.ToList()[1];
            Assert.AreEqual("PathLevelRoot-B", secondPath.ToList()[0]);
            Assert.AreEqual("PathLevel-01-B", secondPath.ToList()[1]);
            Assert.AreEqual("PathLevel-02-B", secondPath.ToList()[2]);

            var thirdPath = xmlPathsFile.XmlPaths.ToList()[2];
            Assert.AreEqual("PathLevelRoot-C", thirdPath.ToList()[0]);
            Assert.AreEqual("PathLevel-01-C", thirdPath.ToList()[1]);
            Assert.AreEqual("PathLevel-02-C", thirdPath.ToList()[2]);
        }
    }
}
