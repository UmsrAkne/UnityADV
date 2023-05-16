﻿using System;

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

        public List<Scenario> Scenarios { get; set; }

        public List<string> Log { get; private set; } = new List<string>();

        public HashSet<string> UsingImageFileNames { get; private set; } = new HashSet<string>();

        public HashSet<string> UsingVoiceFileNames { get; } = new HashSet<string>();

        public HashSet<int> UsingVoiceNumbers { get; } = new HashSet<int>();

        public HashSet<string> UsingBgvFileNames { get; } = new HashSet<string>();

        public Resource Resource { get; set; }

        private List<IXMLElementConverter> Converters { get; set; } = new List<IXMLElementConverter>();

        public void Load(string targetPath)
        {
            targetPath += $@"\{ResourcePath.SceneTextDirectoryName}\scenario.xml";

            if (!File.Exists(targetPath))
            {
                Log.Add($"{targetPath} が見つかりませんでした");
                Scenarios = new List<Scenario>() { new Scenario() { Text = "シナリオの読み込みに失敗しました。" } };
                return;
            }

            try
            {
                XDocument.Parse(File.ReadAllText(targetPath));
            }
            catch (XmlException e)
            {
                Scenarios = new List<Scenario>() { new Scenario() { Text = "シナリオの読み込みに失敗しました。" } };
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

            var scenarioList =
            xml.Root.Descendants()
                .Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario")
                .Where(x => x.Element(ignoreElement) == null).ToList();

            if (scenarioList.FirstOrDefault(x => x.Element("start") != null) != null)
            {
                scenarioList = scenarioList.SkipWhile(x => x.Element("start") == null).ToList();
            }

            Scenarios = scenarioList.Select(x =>
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

            // 使用している画像のファイル名を抽出する
            var targetElements = scenarioList.Descendants()
                .Where(x => x.Name.LocalName == "image" || x.Name.LocalName == "draw" || x.Name.LocalName == "anime");

            UsingImageFileNames = GetUsingImageFileNames(targetElements.ToList());

            // 使用している音声ファイル名と番号を抽出する
            var voiceElements = scenarioList.Descendants()
                .Where(x => x.Name.LocalName == "voice");

            foreach (var v in voiceElements)
            {
                var fileNameAtt = v.Attribute("fileName");
                if (fileNameAtt != null && !string.IsNullOrWhiteSpace(fileNameAtt.Value))
                {
                    UsingVoiceFileNames.Add(fileNameAtt.Value);
                    continue;
                }

                var numberAtt = v.Attribute("number");
                if (numberAtt == null || int.Parse(numberAtt.Value) == 0)
                {
                    continue;
                }

                UsingVoiceNumbers.Add(int.Parse(numberAtt.Value));
            }

            foreach (var bgv in scenarioList.Descendants().Where(x => x.Name.LocalName == "backgroundVoice"))
            {
                var fileNamesAtt = bgv.Attribute("names");
                if (fileNamesAtt != null && !string.IsNullOrWhiteSpace(fileNamesAtt.Value))
                {
                    foreach (var s in fileNamesAtt.Value.Replace(" ", string.Empty).Split(','))
                    {
                        UsingBgvFileNames.Add(s);
                    }
                }
            }

            Converters.ForEach(c => Log.AddRange(c.Log));

            Resource.Scenarios = Scenarios;
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        public HashSet<string> GetUsingImageFileNames(List<XElement> xElements)
        {
            var targetElements = xElements.Descendants()
                .Where(x => x.Name.LocalName == "image" || x.Name.LocalName == "draw" || x.Name.LocalName == "anime");

            var usingImgFileNames = new HashSet<string>();

            foreach (var x in targetElements)
            {
                var a = x.Attribute("a");
                var b = x.Attribute("b");
                var c = x.Attribute("c");
                var d = x.Attribute("d");

                if (a?.Value != null)
                {
                    usingImgFileNames.Add(a.Value);
                }

                if (b?.Value != null)
                {
                    usingImgFileNames.Add(b.Value);
                }

                if (c?.Value != null)
                {
                    usingImgFileNames.Add(c.Value);
                }

                if (d?.Value != null)
                {
                    usingImgFileNames.Add(d.Value);
                }
            }

            return usingImgFileNames;
        }

        public HashSet<string> GetUsingVoiceFileNames(List<XElement> xElements)
        {
            var targetElements = xElements.Descendants("voice");

            var usingVcFileNames = new HashSet<string>();
            foreach (var fileNameAtt in targetElements
                         .Select(v => v.Attribute("fileName"))
                         .Where(fileNameAtt => fileNameAtt != null && !string.IsNullOrWhiteSpace(fileNameAtt.Value)))
            {
                usingVcFileNames.Add(fileNameAtt.Value);
            }

            return usingVcFileNames;
        }
    }
}