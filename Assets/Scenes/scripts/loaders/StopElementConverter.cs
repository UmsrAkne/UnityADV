namespace Loaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class StopElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "stop";

        public List<string> Log => new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement stopTag in tags)
                {
                }
            }
        }
    }
}
