namespace SceneContents
{
    using System.Collections.Generic;
    using Loaders;
    using UnityEngine;

    public class Resource
    {
        public SceneSetting SceneSetting { get; set; } = new SceneSetting();

        public List<string> Log { get; set; } = new List<string>();

        public List<Scenario> Scenarios { get; set; }

        public List<Sprite> Images { get; set; }

        public Dictionary<string, Sprite> ImagesByName { get; set; }

        public List<Sprite> MaskImages { get; set; }

        public Dictionary<string, Sprite> MaskImagesByName { get; set; }

        public AudioSource BGMAudioSource { get; set; }

        public List<ISound> Voices { get; set; }

        public Dictionary<string, ISound> VoicesByName { get; set; }

        public List<ISound> BGVoices { get; set; }

        public Dictionary<string, ISound> BGVoicesByName { get; set; }

        public List<ISound> Ses { get; set; }

        public Sprite MessageWindowImage { get; set; }

        public string SceneDirectoryPath { get; set; }
    }
}
