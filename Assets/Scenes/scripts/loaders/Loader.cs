namespace Scenes.Scripts.Loaders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using SceneContents;
    using UnityEngine;

    public class Loader
    {
        public event EventHandler LoadCompleted;

        private int loadCompleteCount;
        private readonly TextLoader textLoader = new TextLoader();
        private readonly ImageLoader imageLoader = new ImageLoader{TargetImageType = TargetImageType.eventCg};
        private readonly ImageLoader maskLoader = new ImageLoader{TargetImageType = TargetImageType.mask};
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

            voiceLoader.TargetAudioType = TargetAudioType.voice;
            bgvLoader.TargetAudioType = TargetAudioType.bgVoice;
            seLoader.TargetAudioType = TargetAudioType.se;
            bgmLoader.BGMNumber = Resource.SceneSetting.BGMNumber;

            var loaders = new List<IContentsLoader>()
            {
                textLoader,
                imageLoader,
                maskLoader,
                voiceLoader,
                bgvLoader,
                seLoader,
                bgmLoader,
            };

            loaders.ForEach(l =>
            {
                l.LoadCompleted += (sender, e) =>
                {
                    loadCompleteCount++;
                    if (loadCompleteCount >= loaders.Count)
                    {
                        LoadCompleted?.Invoke(this,e);
                    }
                };

                l.Resource = Resource;
                l.Load(path);
            });

            Resource.MessageWindowImage = new ImageLoader().LoadImage($@"{ResourcePath.CommonUIDirectoryName}\msgWindowImage.png").Sprite;
            Resource.SceneDirectoryPath = path;
        }
    }
}