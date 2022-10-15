using NUnit.Framework;

namespace Tests.Loaders
{
    using System.Xml.Linq;
    using Scenes.Scripts.Loaders;

    [TestFixture]
    public class TestSceneSettingLoader
    {

        [Test]
        public void 設定の読み込みのテスト()
        {
            var loader = new SceneSettingLoader();

            const string xmlText =
                @"<setting>
                    <defaultSize width=""1024"" height=""768"" />
                    <bgm number=""3"" />
                </setting>";

            var xDocument = XDocument.Parse(xmlText);
            var setting = loader.LoadSetting(xDocument);

            Assert.AreEqual(setting.DefaultImageWidth, 1024);
            Assert.AreEqual(setting.DefaultImageHeight, 768);
            Assert.AreEqual(setting.BGMNumber, 3);
        }
    }
}