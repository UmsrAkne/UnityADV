using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Scenes.Scripts.Animations;
using Scenes.Scripts.Loaders;
using Scenes.Scripts.SceneContents;

namespace Tests.Loaders
{
    [TestFixture]
    public class TestAnimeElementConverter
    {
        [Test]
        public void 通常生成テスト()
        {
            var converter = new AnimeElementConverter();
        }

        [Test]
        public void アニメーション生成テスト()
        {
            var converter = new AnimeElementConverter();
            var container = new Scenario();
            var xDocument = XDocument.Parse("<scenario><anime name=\"shake\" /></scenario>");
            converter.Convert(xDocument.Element("scenario"), container);

            Assert.IsInstanceOf<Shake>(container.Animations.First());
            Assert.AreEqual(1, container.Animations.Count);
        }
    }
}