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
    }
}