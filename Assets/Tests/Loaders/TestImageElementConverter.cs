namespace Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Loaders;
    using NUnit.Framework;
    using SceneContents;
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
    }
}
