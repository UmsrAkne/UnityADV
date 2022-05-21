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

            Scenario =
            xml.Root.Descendants().Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario").Select(x =>
            {
                var scenario = new Scenario() { Text = x.Element("text").Attribute("str").Value };
                Converters.ForEach(c => c.Convert(x, scenario));
                return scenario;
            }).ToList();

            Converters.ForEach(c => Log.AddRange(c.Log));
        }
    }
}
