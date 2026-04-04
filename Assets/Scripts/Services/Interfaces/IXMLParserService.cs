using System.Xml.Linq;

namespace Services.Interfaces
{
    public interface IXMLParserService
    {
        XDocument Parse(string xml);
    }
}