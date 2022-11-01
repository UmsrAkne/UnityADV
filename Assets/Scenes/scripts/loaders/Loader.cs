namespace Scenes.Scripts.Loaders
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using SceneContents;
    using UnityEngine;

    public class Loader
    {
        public event EventHandler LoadCompleted;

        private readonly TextLoader textLoader = new TextLoader();
        private readonly ImageLoader imageLoader = new ImageLoader();
        private readonly ImageLoader uiLoader = new ImageLoader();
        private readonly ImageLoader maskLoader = new ImageLoader();
        private readonly BGMLoader bgmLoader = new GameObject().AddComponent<BGMLoader>();
        private readonly VoiceLoader voiceLoader = new GameObject().AddComponent<VoiceLoader>();
        private readonly VoiceLoader bgvLoader = new GameObject().AddComponent<VoiceLoader>();
        private readonly VoiceLoader seLoader = new GameObject().AddComponent<VoiceLoader>();

        private readonly SceneSettingLoader sceneSettingLoader = new SceneSettingLoader();

        public Resource Resource { get; set; } = new Resource();

        public void Load(string path)
        {
            var settingXMLPath = $@"{path}\{ResourcePath.SceneTextDirectoryName}\setting.xml";

            if (File.Exists(settingXMLPath))
            {
                Resource.SceneSetting = sceneSettingLoader.LoadSetting(XDocument.Parse(File.ReadAllText(settingXMLPath)));
            }
            else
            {
                Resource.Log.Add("setting.xml を読み込めませんでした");
            }

            textLoader.Load($@"{path}\{ResourcePath.SceneTextDirectoryName}\scenario.xml");

            imageLoader.Load($@"{path}\{ResourcePath.SceneImageDirectoryName}");
            maskLoader.Load($@"{path}\{ResourcePath.SceneMaskImageDirectoryName}");
            voiceLoader.Load($@"{path}\{ResourcePath.SceneVoiceDirectoryName}");
            bgvLoader.Load($@"{path}\{ResourcePath.SceneBgvDirectoryName}");
            seLoader.Load($@"{ResourcePath.CommonSeDirectoryName}");

            bgmLoader.BGMNumber = Resource.SceneSetting.BGMNumber;
            bgmLoader.Load($@"{ResourcePath.CommonBGMDirectoryName}");
            bgmLoader.LoadCompleted += (sender, e) =>  LoadCompleted?.Invoke(this, e);

            Resource.Scenarios = textLoader.Scenario;
            Resource.Log.AddRange(textLoader.Log);

            Resource.Images = imageLoader.Sprites;
            Resource.ImagesByName = imageLoader.SpriteDictionary;
            Resource.Log.AddRange(imageLoader.Log);

            Resource.MaskImages = maskLoader.Sprites;
            Resource.MaskImagesByName = maskLoader.SpriteDictionary;
            Resource.Log.AddRange(maskLoader.Log);

            Resource.BGMAudioSource = bgmLoader.AudioSource;
            Resource.Log.AddRange(bgmLoader.Log);

            Resource.Voices = voiceLoader.AudioSources;
            Resource.VoicesByName = voiceLoader.AudioSourcesByName;
            Resource.Log.AddRange(voiceLoader.Log);

            Resource.BGVoices = bgvLoader.AudioSources;
            Resource.BGVoicesByName = bgvLoader.AudioSourcesByName;
            Resource.Log.AddRange(bgvLoader.Log);

            Resource.Ses = seLoader.AudioSources;
            Resource.Log.AddRange(seLoader.Log);

            Resource.MessageWindowImage = uiLoader.LoadImage($@"{ResourcePath.CommonUIDirectoryName}\msgWindowImage.png").Sprite;
            Resource.SceneDirectoryPath = path;
        }
    }
}