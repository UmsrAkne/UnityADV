namespace MainLogics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Animations;
    using SceneContents;
    using SceneParts;
    using UnityEngine;

    public class ScenarioScene : MonoBehaviour
    {
        public Resource Resource { private get; set; } = new Resource();

        private List<IScenarioSceneParts> ScenarioSceneParts { get; } = new List<IScenarioSceneParts>();

        private TextWriter TextWriter { get; } = new TextWriter();

        private UI UI { get; } = new UI();

        // Start is called before the first frame update
        public void Start()
        {
            InjectUI(UI);

            ScenarioSceneParts.Add(TextWriter);

            ScenarioSceneParts.Add(new ImageDrawer());

            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[0]));
            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[1]));
            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[2]));

            ScenarioSceneParts.Add(new BGMPlayer());
            ScenarioSceneParts.Add(new VoicePlayer());
            ScenarioSceneParts.Add(new SEPlayer());

            ScenarioSceneParts.ForEach(s =>
            {
                s.SetResource(Resource);
                s.SetUI(UI);
            });

            InvokeRepeating(nameof(ExecuteEveryFrames), 0, 0.025f);
        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Forward();
            }
        }

        public void Forward()
        {
            ScenarioSceneParts.ForEach(p => p.SetScenario(Resource.Scenarios[TextWriter.ScenarioIndex]));
            ScenarioSceneParts.ForEach(p => p.Execute());
        }

        private void InjectUI(UI ui)
        {
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_0"), Index = 0 });
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_1"), Index = 1 });
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_2"), Index = 2 });
        }

        private void ExecuteEveryFrames()
        {
            ScenarioSceneParts.ForEach(p => p.ExecuteEveryFrame());
        }
    }
}
