using System.Xml.Linq;
using Services.Interfaces;

namespace Services.Implementations
{
    public class XMLParserService : IXMLParserService
    {
        public XDocument Parse(string xml)
        {
            return XDocument.Parse(xml);
        }
    }
}