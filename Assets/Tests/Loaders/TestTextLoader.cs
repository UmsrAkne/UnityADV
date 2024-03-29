using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Scenes.Scripts.Loaders;

namespace Tests.Loaders
{
    [TestFixture]
    public class TestTextLoader
    {
        [Test]
        public void GetUsingImageFileNames()
        {
            var loader = new TextLoader();
            var elements = new List<XElement>()
            {
                XElement.Parse(@"<scenario></scenario>"),
                XElement.Parse(@"<scenario><image a=""testA"" b=""testB"" /></scenario>"),
                XElement.Parse(@"<scenario><draw  a=""testD"" b=""testE"" /></scenario>"),
                XElement.Parse(@"<scenario><anime name=""draw"" a=""testF"" b=""testG"" /></scenario>"),
            };

            var names = loader.GetUsingImageFileNames(elements);
            Assert.IsTrue(names.Contains("testA"));
            Assert.IsTrue(names.Contains("testB"));

            Assert.IsTrue(names.Contains("testD"));
            Assert.IsTrue(names.Contains("testE"));

            Assert.IsTrue(names.Contains("testF"));
            Assert.IsTrue(names.Contains("testG"));

            Assert.AreEqual(6, names.Count);
        }

        [Test]
        public void GetUsingVoiceFileNames()
        {
            var loader = new TextLoader();

            var elements = new List<XElement>()
            {
                XElement.Parse(@"<scenario></scenario>"),
                XElement.Parse(@"<scenario><voice fileName=""vc1"" /></scenario>"),
                XElement.Parse(@"<scenario><voice fileName=""vc2"" /></scenario>"),
                XElement.Parse(@"<scenario><voice number=""00000"" /></scenario>"),
            };

            var names = loader.GetUsingVoiceFileNames(elements);
            Assert.IsTrue(names.Contains("vc1"));
            Assert.IsTrue(names.Contains("vc2"));
            Assert.AreEqual(2, names.Count);
        }

        [Test]
        public void GetUsingVoiceNumbers()
        {
            var loader = new TextLoader();

            var elements = new List<XElement>()
            {
                XElement.Parse(@"<scenario></scenario>"),
                XElement.Parse(@"<scenario><voice fileName=""vc1"" /></scenario>"),
                XElement.Parse(@"<scenario><voice number=""00000"" /></scenario>"),
                XElement.Parse(@"<scenario><voice number=""1"" /></scenario>"),
                XElement.Parse(@"<scenario><voice number=""002"" /></scenario>"),
            };

            var numbers = loader.GetUsingVoiceNumbers(elements);
            Assert.IsFalse(numbers.Contains(0), "０番は例外的に除外する仕様とするので、リストに含まれないのが正しい");
            Assert.IsTrue(numbers.Contains(1));
            Assert.IsTrue(numbers.Contains(2));
            Assert.AreEqual(2, numbers.Count);
        }

        [Test]
        public void GetUsingBgvFileNames()
        {
            var loader = new TextLoader();

            var elements = new List<XElement>()
            {
                XElement.Parse(@"<scenario></scenario>"),
                XElement.Parse(@"<scenario><backgroundVoice names=""aa, bb,  cc, dd""/></scenario>"),
                XElement.Parse(@"<scenario><bgv names=""ee, ff,  gg, hh""/></scenario>"),
            };

            var names = loader.GetUsingBgvFileNames(elements);
            Assert.IsTrue(names.Contains("aa"));
            Assert.IsTrue(names.Contains("bb"));
            Assert.IsTrue(names.Contains("cc"));
            Assert.IsTrue(names.Contains("dd"));

            Assert.IsTrue(names.Contains("ee"));
            Assert.IsTrue(names.Contains("ff"));
            Assert.IsTrue(names.Contains("gg"));
            Assert.IsTrue(names.Contains("hh"));

            Assert.AreEqual(8, names.Count);
        }
    }
}