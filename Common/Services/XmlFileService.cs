using Common.Entities;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System;

namespace Common.Services
{
    public class XmlFileService
    {
        public XmlFileEntity LoadXmlFile(string fileName)
        {
            var xmlDoc = XDocument.Load(fileName);
            var rootNode = new XmlNodeEntity(xmlDoc.Root.Name.LocalName, xmlDoc.Root.Value);
            var xmlFileEntity = new XmlFileEntity(rootNode);

            var childrenNodesFromRoot = GetChildrenNodes(xmlDoc.Root);
            childrenNodesFromRoot.ForEach(x => xmlFileEntity.RootNode.AddChildNode(x));

            return xmlFileEntity;
        }

        List<XmlNodeEntity> GetChildrenNodes(XElement parentElement)
        {
            var children = new List<XmlNodeEntity>();
            foreach (var oneElt in parentElement.Elements())
            {
                // adds children nodes of the current XElement to the new entity
                var childrenList = GetChildrenNodes(oneElt);
                var newNodeEntity = new XmlNodeEntity(oneElt.Name.LocalName, oneElt.Value);
                childrenList
                    .ForEach(x => newNodeEntity.AddChildNode(x));

                // adds attributes of the current XElement to the new entity
                var attributes = GetListOfAttributesFromXelement(oneElt);
                attributes
                    .ToList()
                    .ForEach(x => newNodeEntity.AddAttribute(x.Key, x.Value));
                children.Add(newNodeEntity);
            }
            return children;
        }

        Dictionary<string, string> GetListOfAttributesFromXelement(XElement xElement)
        {
            var result = new Dictionary<string, string>();

            if (!xElement.HasAttributes)
                return result;

            foreach (var oneAttribute in xElement.Attributes())
            {
                var attrKey = oneAttribute.Name.LocalName;
                var attrValue = oneAttribute.Value;
                result.Add(attrKey, attrValue);
            }
            return result;
        }

        public bool CreateOutputMissingInSourceFile(IEnumerable<string> listOfMissings, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                throw new Exception($"The output folder you specified doesn't exist:\n{outputFolder}");

            var outputFileName = Path.Combine(outputFolder, "output.txt");
            File.WriteAllLines(outputFileName, listOfMissings);
            return true;
        }
    }
}
