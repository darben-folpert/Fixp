using System.Collections.Generic;

namespace Common.Entities
{
    public class XmlNodeEntity
    {
        string name;
        string value;
        List<XmlNodeEntity> childrenNodes;
        XmlNodeEntity parentNode;
        Dictionary<string, string> attributes;

        public XmlNodeEntity(string nodeName, string value)
        {
            name = nodeName;
            this.value = value;
            childrenNodes = new List<XmlNodeEntity>();
            attributes = new Dictionary<string, string>();
            parentNode = null;
        }

        public List<XmlNodeEntity> ChildrenNodes
        {
            get { return childrenNodes; }
        }

        public Dictionary<string, string> Attributes
        {
            get { return attributes; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Value
        {
            get { return value; }
        }

        public XmlNodeEntity Parent
        {
            get { return parentNode; }
        }

        public void AddChildNode(XmlNodeEntity newNode)
        {
            newNode.parentNode = this;
            childrenNodes.Add(newNode);
        }

        public void AddAttribute(string key, string value)
        {
            attributes.Add(key, value);
        }
    }
}
