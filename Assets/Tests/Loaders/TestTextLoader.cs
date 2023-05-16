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
    }
}