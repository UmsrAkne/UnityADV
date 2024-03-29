﻿namespace Scenes.Scripts.SceneParts
{
    using Loaders;
    using Scenes.Scripts.SceneContents;
    using UnityEngine;

    public class BGMPlayer : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame => false;

        private AudioSource BGM { get; set; }

        private bool Playing { get; set; }

        private SceneSetting SceneSetting { get; set; }

        public void Execute()
        {
            if (!Playing)
            {
                BGM.loop = true;
                BGM.volume = SceneSetting.BGMVolume;
                BGM.Play();
                Playing = true;
            }
        }

        public void ExecuteEveryFrame()
        {
            // throw new System.NotImplementedException();
        }

        public void SetResource(Resource resource)
        {
            BGM = resource.BGMAudioSource;
            SceneSetting = resource.SceneSetting;
        }

        public void SetScenario(Scenario scenario)
        {
        }

        public void SetUI(UI ui)
        {
        }
    }
}