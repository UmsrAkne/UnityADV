using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Scenes.Scripts.Loaders;
using Scenes.Scripts.SceneContents;

namespace Tests.Loaders
{
    public class TestBackgroundVoiceElementConverter
    {
        [Test]
        public void ファイル名複数読み込みのテスト()
        {
            var converter = new BackgroundVoiceElementConverter();
            var xml = XDocument.Parse("<scenario> <backgroundVoice names=\"file01, file02, file03\" /> </scenario>");
            var scenarioElement = xml.Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.BGVOrders.Count, 1);
            CollectionAssert.AreEqual(new List<string> { "file01", "file02", "file03" }, scenario.BGVOrders[0].FileNames);
        }

        [Test]
        public void ファイル名単体読み込みテスト()
        {
            var converter = new BackgroundVoiceElementConverter();
            var xml = XDocument.Parse("<scenario> <backgroundVoice names=\"file01\" /> </scenario>");
            var scenarioElement = xml.Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.BGVOrders.Count, 1);
            CollectionAssert.AreEqual(new List<string> { "file01" }, scenario.BGVOrders[0].FileNames);
        }

        [Test]
        public void チャンネルの読み込みテスト()
        {
            var converter = new BackgroundVoiceElementConverter();
            var xml = XDocument.Parse("<scenario> <backgroundVoice names=\"file01\" channel=\"1\"/> </scenario>");
            var scenario = new Scenario();
            converter.Convert(xml.Root, scenario);

            Assert.AreEqual(scenario.BGVOrders.Count, 1);
            Assert.AreEqual(scenario.BGVOrders[0].Channel, 1);
        }

        [Test]
        public void 複数オーダーの読み込み()
        {
            var xml = XDocument.Parse("<scenario> <backgroundVoice names=\"file01\" /> <backgroundVoice names=\"file02\" channel=\"1\"/> </scenario>");
            var scenario = new Scenario();
            new BackgroundVoiceElementConverter().Convert(xml.Root, scenario);

            Assert.AreEqual(scenario.BGVOrders.Count, 2);
            Assert.AreEqual(scenario.BGVOrders[0].Channel, 0);
            Assert.AreEqual(scenario.BGVOrders[1].Channel, 1);

            CollectionAssert.AreEqual(new List<string> { "file01" }, scenario.BGVOrders[0].FileNames);
            CollectionAssert.AreEqual(new List<string> { "file02" }, scenario.BGVOrders[1].FileNames);
        }
    }
}