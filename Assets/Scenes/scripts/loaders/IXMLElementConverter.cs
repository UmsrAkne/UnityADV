namespace Loaders
{
    using System.Xml.Linq;
    using SceneContents;

    public interface IXMLElementConverter
    {
        string TargetElementName { get; }

        void Convert(XElement xmlElement, Scenario scenario);
    }
}
