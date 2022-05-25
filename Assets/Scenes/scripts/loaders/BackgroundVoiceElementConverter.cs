namespace Loaders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class BackgroundVoiceElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "backgroundVoice";

        public List<string> Log => new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement bgvTag in tags)
                {
                }
            }
        }
    }
}
