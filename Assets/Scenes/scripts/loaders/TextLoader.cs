using System;

namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using SceneContents;

    public class TextLoader : IContentsLoader
    {
        public event EventHandler LoadCompleted;

        private string textAttribute = "text";
        private string stringAttribute = "string";
        private string strAttribute = "str";
        private string ignoreElement = "ignore";

        public List<Scenario> Scenario { get; set; }

        public List<string> Log { get; private set; } = new List<string>();

        public Resource Resource { get; set; }

        private List<IXMLElementConverter> Converters { get; set; } = new List<IXMLElementConverter>();

        public void Load(string targetPath)
        {
            targetPath += $@"\{ResourcePath.SceneTextDirectoryName}\scenario.xml";

            if (!File.Exists(targetPath))
            {
                Log.Add($"{targetPath} が見つかりませんでした");
                Scenario = new List<Scenario>() { new Scenario() { Text = "シナリオの読み込みに失敗しました。" } };
                return;
            }

            try
            {
                XDocument.Parse(File.ReadAllText(targetPath));
            }
            catch (XmlException e)
            {
                Scenario = new List<Scenario>() { new Scenario() { Text = "シナリオの読み込みに失敗しました。" } };
                Log.Add($"scenario.xmlのパースに失敗しました。詳細 : {e.Message}");
                return;
            }

            Converters.Add(new ChapterElementConverter());
            Converters.Add(new ImageElementConverter());
            Converters.Add(new DrawElementConverter());
            Converters.Add(new VoiceElementConverter());
            Converters.Add(new SEElementConverter());
            Converters.Add(new AnimeElementConverter());
            Converters.Add(new BackgroundVoiceElementConverter());
            Converters.Add(new StopElementConverter());

            XDocument xml = XDocument.Parse(File.ReadAllText(targetPath));

            var scenarioIndex = 0;

            Scenario =
            xml.Root.Descendants()
                .Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario")
                .Where(x => x.Element(ignoreElement) == null)
                .Select(x =>
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

            Resource.Scenarios = Scenario;
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}