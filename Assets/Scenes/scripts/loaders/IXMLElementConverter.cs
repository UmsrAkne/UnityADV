using System.Xml.Linq;
using SceneContents;

namespace Loaders
{
    public interface IXMLElementConverter
    {
        string TargetElementName { get; }

        void Convert(XElement xmlElement, Scenario scenario);
    }
}
