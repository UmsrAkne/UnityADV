namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class SceneSettingLoader
    {
        private string defaultSizeElementName = "defaultSize";
        private string bgmElementName = "bgm";

        private string widthAttribute = "width";
        private string heihgtAttribute = "height";
        private string numberAttribute = "number";

        public List<string> Log { get; private set; } = new List<string>();

        public SceneSetting LoadSetting(XDocument xml)
        {
            var setting = new SceneSetting();
            XElement settingTag = xml.Root;

            if (settingTag.Element(defaultSizeElementName) != null)
            {
                setting.DefaultImageWidth = (int)settingTag.Element(defaultSizeElementName).Attribute(widthAttribute);
                setting.DefaultImageHeight = (int)settingTag.Element(defaultSizeElementName).Attribute(heihgtAttribute);
            }

            if (settingTag.Element(bgmElementName) != null)
            {
                setting.BGMNumber = int.Parse(settingTag.Element(bgmElementName).Attribute(numberAttribute).Value);
            }

            return setting;
        }
    }
}
