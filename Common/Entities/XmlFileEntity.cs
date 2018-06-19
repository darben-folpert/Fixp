
namespace Common.Entities
{
    public class XmlFileEntity
    {
        readonly XmlNodeEntity rootNode;

        public XmlFileEntity(XmlNodeEntity rootNode)
        {
            this.rootNode = rootNode;
        }

        public XmlNodeEntity RootNode
        {
            get { return rootNode; }
        }
    }
}
