using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Services;
using Common.Entities;

namespace Test.Common.Services
{
    [TestClass]
    public class XmlFileServiceTest
    {
        readonly XmlFileEntity xmlFileEntity01;
        readonly XmlFileEntity xmlFileEntity02;

        public XmlFileServiceTest()
        {
            var xmlFileService = new XmlFileService();
            xmlFileEntity01 = xmlFileService.LoadXmlFile(@"Data\Services\01\data01.xml");
            xmlFileEntity02 = xmlFileService.LoadXmlFile(@"Data\Services\01\data02.xml");
        }

        [TestMethod]
        public void ShouldLoadXmlFiles()
        {
            Assert.IsNotNull(xmlFileEntity01);
            Assert.IsNotNull(xmlFileEntity02);
        }

        [TestMethod]
        public void ShouldContainCorrectRootElement()
        {
            Assert.AreEqual("LegalEntityExtenedStructure", xmlFileEntity01.RootNode.Name);
        }

        [TestMethod]
        public void ShouldLoadDirectChildren()
        {
            var nbChildren = xmlFileEntity01.RootNode.ChildrenNodes.Count;
            Assert.AreEqual(18, nbChildren);
        }

        [TestMethod]
        public void ShouldFullyLoadData()
        {
            var nodeShareHolderList = xmlFileEntity02.RootNode.ChildrenNodes[4];
            var nodeShareHolder = nodeShareHolderList.ChildrenNodes[0];
            var nodeLegalPerson = nodeShareHolder.ChildrenNodes[2];
            var nodeLatvianEnterprise = nodeLegalPerson.ChildrenNodes[0];
            var nodeLegalAddress = nodeLatvianEnterprise.ChildrenNodes[1];
            var nodeARCode = nodeLegalAddress.ChildrenNodes[0];
            Assert.AreEqual("101984047", nodeARCode.Value);
        }

        [TestMethod]
        public void ShouldLoadCorrectCountOfAttributes()
        {
            var fixedCapitalNode = xmlFileEntity01.RootNode.ChildrenNodes[8];
            var registeredNode = fixedCapitalNode.ChildrenNodes[0];
            var amountNode = registeredNode.ChildrenNodes[1];
            var currencyNode = amountNode.ChildrenNodes[1];
            Assert.AreEqual(3, currencyNode.Attributes.Count);
        }

        [TestMethod]
        public void ShouldLoadRightValueForAttributes()
        {
            var fixedCapitalNode = xmlFileEntity01.RootNode.ChildrenNodes[8];
            var registeredNode = fixedCapitalNode.ChildrenNodes[0];
            var amountNode = registeredNode.ChildrenNodes[1];
            var currencyNode = amountNode.ChildrenNodes[1];
            Assert.AreEqual("100009", currencyNode.Attributes["CodeListAgencyID"]);
            Assert.AreEqual("ISO4217", currencyNode.Attributes["CodeListID"]);
            Assert.AreEqual("0.3", currencyNode.Attributes["CodeListVersionID"]);
        }
    }
}
