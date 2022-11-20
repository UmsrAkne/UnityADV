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

            textLoader.Resource = Resource;
            textLoader.Load(path);

            imageLoader.TargetImageType = TargetImageType.eventCg;
            imageLoader.Resource = Resource;
            imageLoader.Load(path);

            maskLoader.TargetImageType = TargetImageType.mask;
            maskLoader.Resource = Resource;
            maskLoader.Load(path);

            voiceLoader.TargetAudioType = TargetAudioType.voice;
            voiceLoader.Resource = Resource;
            voiceLoader.Load(path);

            bgvLoader.TargetAudioType = TargetAudioType.bgVoice;
            bgvLoader.Resource = Resource;
            bgvLoader.Load(path);

            seLoader.TargetAudioType = TargetAudioType.se;
            seLoader.Resource = Resource;
            seLoader.Load(path);

            Resource.MessageWindowImage = uiLoader.LoadImage($@"{ResourcePath.CommonUIDirectoryName}\msgWindowImage.png").Sprite;

            bgmLoader.BGMNumber = Resource.SceneSetting.BGMNumber;
            bgmLoader.Resource = Resource;
            bgmLoader.LoadCompleted += (sender, e) => LoadCompleted?.Invoke(this, e);
            bgmLoader.Load($@"{ResourcePath.CommonBGMDirectoryName}");

            Resource.SceneDirectoryPath = path;
        }
    }
}