﻿namespace SceneContents
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Resource
    {
        public List<string> Log { get; set; }

        public List<Scenario> Scenarios { get; set; }

        public List<Sprite> Images { get; set; }

        public Dictionary<string, Sprite> ImagesByName { get; set; }

        public List<Sprite> MaskImages { get; set; }

        public Dictionary<string, Sprite> MaskImagesByName { get; set; }

        public AudioSource BGMAudioSource { get; set; }

        public List<ISound> Voices { get; set; }

        public List<ISound> Ses { get; set; }

        public Sprite MessageWindowImage { get; set; }
    }
}
