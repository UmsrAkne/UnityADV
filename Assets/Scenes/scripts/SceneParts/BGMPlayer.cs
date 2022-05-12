using UnityEngine;
using UnityEngine.Audio;
using SceneContents;

namespace sceneParts
{
    public class BGMPlayer : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame => false;

        private AudioSource BGM { get; set; }

        private bool Playing { get; set; }

        public void Execute()
        {
            if (!Playing)
            {
                BGM.loop = true;
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
        }

        public void SetScenario(Scenario scenario)
        {
        }

        public void SetUI(UI ui)
        {
        }
    }
}
