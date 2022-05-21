namespace Loaders
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using SceneContents;
    using UnityEngine;

    public class Loader
    {
        private TextLoader textLoader = new TextLoader();
        private ImageLoader imageLoader = new ImageLoader();
        private ImageLoader uiLoader = new ImageLoader();
        private ImageLoader maskLoader = new ImageLoader();
        private BGMLoader bgmLoader = new GameObject().AddComponent<BGMLoader>();
        private VoiceLoader voiceLoader = new GameObject().AddComponent<VoiceLoader>();
        private VoiceLoader seLoader = new GameObject().AddComponent<VoiceLoader>();

        public Resource Resource { get; set; } = new Resource();

        public void Load(string path)
        {
            textLoader.Load($@"{path}\texts\scenario.xml");

            imageLoader.Load($@"{path}\images", 1280, 720);
            maskLoader.Load($@"{path}\masks", 1280, 720);
            voiceLoader.Load($@"{path}\voices");
            bgmLoader.Load(@"commonResource\bgms");
            seLoader.Load(@"commonResource\ses");

            Resource.Scenarios = textLoader.Scenario;
            Resource.Log = textLoader.Log;
            Resource.Images = imageLoader.Sprites;
            Resource.ImagesByName = imageLoader.SpriteDictionary;

            Resource.MaskImages = maskLoader.Sprites;
            Resource.MaskImagesByName = maskLoader.SpriteDictionary;

            Resource.BGMAudioSource = bgmLoader.AudioSource;
            Resource.Voices = voiceLoader.AudioSources;
            Resource.Ses = seLoader.AudioSources;

            Resource.MessageWindowImage = uiLoader.LoadImage(@"commonResource\uis\msgWindowImage.png", 800, 150);
        }
    }
}
