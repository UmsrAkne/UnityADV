namespace Loaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Animations;
    using SceneContents;
    using UnityEngine;

    public class TextLoader
    {
        private string textAttribute = "text";
        private string stringAttribute = "string";
        private string strAttribute = "str";

        public List<Scenario> Scenario { get; set; }

        public List<string> Log { get; private set; } = new List<string>();

        private List<IXMLElementConverter> Converters { get; set; } = new List<IXMLElementConverter>();

        public void Load(string targetPath)
        {
            Converters.Add(new ImageElementConverter());
            Converters.Add(new DrawElementConverter());
            Converters.Add(new VoiceElementConverter());
            Converters.Add(new SEElementConverter());
            Converters.Add(new AnimeElementConverter());

            XDocument xml = XDocument.Parse(File.ReadAllText(targetPath));

            var scenarioIndex = 0;

            Scenario =
            xml.Root.Descendants().Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario").Select(x =>
            {
                var scenario = new Scenario() { Index = ++scenarioIndex };

                if (x.Element(textAttribute).Attribute(strAttribute) != null)
                {
                    scenario.Text = x.Element(textAttribute).Attribute(strAttribute).Value;
                }

                if (x.Element(textAttribute).Attribute(stringAttribute) != null)
                {
                    scenario.Text = x.Element(textAttribute).Attribute(stringAttribute).Value;
                }

                Converters.ForEach(c => c.Convert(x, scenario));
                return scenario;
            }).ToList();

            Converters.ForEach(c => Log.AddRange(c.Log));
        }
    }
}
