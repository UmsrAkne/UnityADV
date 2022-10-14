namespace Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using NUnit.Framework;
    using Scenes.Scripts.Loaders;
    using Scenes.Scripts.SceneContents;
    using UnityEngine.TestTools;

    public class TestImageElementConverter
    {
        [Test]
        public void ABCD属性の読み込みテスト()
        {
            var converter = new ImageElementConverter();
            var scenarioElement = XDocument.Parse("<scenario> <image a=\"A01\" b=\"B02\" c=\"C03\" d=\"D04\" /> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.ImageOrders.Count, 1);
            Assert.AreEqual(scenario.ImageOrders[0].Names.Count, 4);
            CollectionAssert.AreEqual(scenario.ImageOrders[0].Names, new List<string> { "A01", "B02", "C03", "D04" });
        }

        [TestCase("<scenario> <image a=\"A01\" /> </scenario>", new string[] { "A01", "", "", "" })]
        [TestCase("<scenario> <image b=\"B01\" /> </scenario>", new string[] { "", "B01", "", "" })]
        [TestCase("<scenario> <image c=\"C01\" /> </scenario>", new string[] { "", "", "C01", "" })]
        [TestCase("<scenario> <image d=\"D01\" /> </scenario>", new string[] { "", "", "", "D01" })]
        [TestCase("<scenario> <image a=\"A01\" b=\"B01\" /> </scenario>", new string[] { "A01", "B01", "", "" })]
        [TestCase("<scenario> <image b=\"B01\" c=\"C01\" /> </scenario>", new string[] { "", "B01", "C01", "" })]
        [TestCase("<scenario> <image c=\"C01\" d=\"D01\" /> </scenario>", new string[] { "", "", "C01", "D01" })]
        [TestCase("<scenario> <image d=\"D01\" a=\"A01\" /> </scenario>", new string[] { "A01", "", "", "D01" })]
        public void ABCD属性の一部分の読み込みテスト(string xmlText, string[] names)
        {
            var converter = new ImageElementConverter();
            var scenarioElement = XDocument.Parse(xmlText).Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.ImageOrders.Count, 1);
            Assert.AreEqual(scenario.ImageOrders[0].Names.Count, 4);
            CollectionAssert.AreEqual(scenario.ImageOrders[0].Names, new List<string>(names));
        }

        [Test]
        public void プロパティ読み取りのテスト()
        {
            var converter = new ImageElementConverter();
            var scenarioElement = XDocument.Parse("<scenario> <image a=\"A01\" x=\"100\" y=\"200\" angle=\"10\" scale=\"2.0\" mask=\"maskImageName\"/> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.ImageOrders.Count, 1);

            Assert.AreEqual(scenario.ImageOrders.First().X, 100);
            Assert.AreEqual(scenario.ImageOrders.First().Y, 200);
            Assert.AreEqual(scenario.ImageOrders.First().Angle, 10);
            Assert.AreEqual(scenario.ImageOrders.First().Scale, 2.0);
            Assert.AreEqual(scenario.ImageOrders.First().MaskImageName, "maskImageName");
        }

        [Test]
        public void 複数命令の読み込みテスト()
        {
            var converter = new ImageElementConverter();
            var scenarioElement = XDocument.Parse("<scenario> <image a=\"A01\" /> <image a=\"A02\" /> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.ImageOrders.Count, 2);
            CollectionAssert.AreEqual(scenario.ImageOrders[0].Names, new List<string>() { "A01", string.Empty, string.Empty, string.Empty });
            CollectionAssert.AreEqual(scenario.ImageOrders[1].Names, new List<string>() { "A02", string.Empty, string.Empty, string.Empty });
        }
    }
}
