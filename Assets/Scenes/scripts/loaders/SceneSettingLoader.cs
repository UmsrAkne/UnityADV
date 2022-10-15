namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class SceneSettingLoader
    {
        private readonly string defaultSizeElementName = "defaultSize";
        private readonly string bgmElementName = "bgm";

        private readonly string widthAttribute = "width";
        private readonly string heightAttribute = "height";
        private readonly string numberAttribute = "number";

        public List<string> Log { get; private set; } = new List<string>();

        public SceneSetting LoadSetting(XDocument xml)
        {
            var setting = new SceneSetting();
            var settingTag = xml.Root;

            if (settingTag == null)
            {
                return setting;
            }

            if (settingTag.Element(defaultSizeElementName) != null)
            {
                var w = settingTag.Element(defaultSizeElementName)?.Attribute(widthAttribute)?.Value;
                setting.DefaultImageWidth = w != null ? int.Parse(w) : setting.DefaultImageWidth;

                var h = settingTag.Element(defaultSizeElementName)?.Attribute(heightAttribute)?.Value;
                setting.DefaultImageHeight = h != null ? int.Parse(h) : setting.DefaultImageHeight;
            }

            if (settingTag.Element(bgmElementName) != null)
            {
                var bgmNumber = settingTag.Element(bgmElementName)?.Attribute(numberAttribute)?.Value;
                setting.BGMNumber = bgmNumber != null ? int.Parse(bgmNumber) : setting.BGMNumber;
            }

            return setting;
        }
    }
}