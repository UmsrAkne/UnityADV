using System.Xml.Linq;

public interface IXMLElementConverter
{
    string TargetElementName { get; }

    void Convert(XElement xmlElement, Scenario scenario);
}
