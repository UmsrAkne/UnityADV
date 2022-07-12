namespace Loaders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;
    using UnityEngine;

    public class ChapterElementConverter : IXMLElementConverter
    {
        private readonly string nameAttribute = "name";

        public string TargetElementName => "chapter";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement chapterTag in tags)
                {
                    if (chapterTag.Attribute(nameAttribute) != null)
                    {
                        scenario.ChapterName = chapterTag.Attribute(nameAttribute).Value;
                    }
                }
            }
        }
    }
}
