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

        [Test]
        public void アニメーションチェーン生成テスト()
        {
            var converter = new AnimeElementConverter();
            var container = new Scenario();
            var xmlText = "<scenario>" +
                            "<anime name=\"animationChain\" repeatCount=\"10\">" +
                              "<anime name=\"shake\" />" +
                              "<anime name=\"slide\" />" +
                            "</anime>" +
                          "</scenario>";

            var xDocument = XDocument.Parse(xmlText);
            converter.Convert(xDocument.Element("scenario"), container);

            Assert.IsInstanceOf<AnimationChain>(container.Animations.First());
            Assert.AreEqual(1, container.Animations.Count);

            var chain = container.Animations.First() as AnimationChain;
            Assert.AreEqual(chain.AnimeTags[0].ToString(), "<anime name=\"shake\" />");
            Assert.AreEqual(chain.AnimeTags[1].ToString(), "<anime name=\"slide\" />");
            Assert.AreEqual(chain.RepeatCount, 10);
        }
    }
}