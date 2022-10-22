using System.Collections.Generic;
using Scenes.Scripts.Loaders;
using Scenes.Scripts.SceneContents;
using Scenes.Scripts.SceneParts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Scripts.MainLogics
{
    using Animations;

    public class ScenarioScene : MonoBehaviour
    {
        private GameObject logWindowObject;
        private Scenario currentScenario;

        public Resource Resource { private get; set; } = new Resource();

        private List<IScenarioSceneParts> ScenarioSceneParts { get; } = new List<IScenarioSceneParts>();

        private TextWriter TextWriter { get; } = new TextWriter();
        private ChapterManager ChapterManager { get; } = new ChapterManager();

        private UI UI { get; } = new UI();

        // Start is called before the first frame update
        public void Start()
        {
            logWindowObject = GameObject.Find("LogTextField");
            Resource.Log.ForEach(t => logWindowObject.GetComponent<Text>().text += $"{t}\n");
            logWindowObject.GetComponent<Text>().text += "ロード完了";

            InjectUI(UI);

            ScenarioSceneParts.Add(new ImageDrawer());

            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[0]));
            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[1]));
            ScenarioSceneParts.Add(new AnimationsManager(UI.ImageContainers[2]));

            ScenarioSceneParts.Add(new BGMPlayer());

            var vp1 = new VoicePlayer() { Channel = 0 };
            var vp2 = new VoicePlayer() { Channel = 1 };
            var vp3 = new VoicePlayer() { Channel = 2 };

            ScenarioSceneParts.Add(vp1);
            ScenarioSceneParts.Add(vp2);
            ScenarioSceneParts.Add(vp3);

            ScenarioSceneParts.Add(new BGVoicePlayer(vp1));
            ScenarioSceneParts.Add(new BGVoicePlayer(vp2));
            ScenarioSceneParts.Add(new BGVoicePlayer(vp3));

            ScenarioSceneParts.Add(new SEPlayer());

            ScenarioSceneParts.Add(ChapterManager);

            ScenarioSceneParts.ForEach(s =>
            {
                s.SetResource(Resource);
                s.SetUI(UI);
            });

            TextWriter.SetResource(Resource);
            TextWriter.SetUI(UI);

            InvokeRepeating(nameof(ExecuteEveryFrames), 0, 0.025f);
        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Forward();

                if (logWindowObject != null)
                {
                    Destroy(logWindowObject);
                    logWindowObject = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                TextWriter.SetScenarioIndex(ChapterManager.GetNextChapterIndex());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.sceneLoaded += LoadSceneResource;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        private void Forward()
        {
            TextWriter.Execute();

            if (!TextWriter.Writing)
            {
                return;
            }

            if (currentScenario == null || currentScenario != Resource.Scenarios[TextWriter.ScenarioIndex])
            {
                currentScenario = Resource.Scenarios[TextWriter.ScenarioIndex];
                ScenarioSceneParts.ForEach(p => p.SetScenario(currentScenario));
                ScenarioSceneParts.ForEach(p => p.Execute());
            }
        }

        private void InjectUI(UI ui)
        {
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_0"), Index = 0 });
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_1"), Index = 1 });
            ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_2"), Index = 2 });

            ui.ImageContainers[0].AddEffectLayer();
            ui.ImageContainers[1].AddEffectLayer();
            ui.ImageContainers[2].AddEffectLayer();

            ui.UIImageContainer = new ImageContainer() { GameObject = GameObject.Find("ImageContainer_ui"), Index = 3 };

            //// ここからメッセージウィンドウを表示するためのコード

            var imageSet = new ImageSet
            {
                GameObject =
                {
                    name = "msgWindowImage"
                }
            };

            var r = imageSet.SetSprite(Resource.MessageWindowImage, 0);
            r.maskInteraction = SpriteMaskInteraction.None;
            r.color = new Color(1f, 1f, 1f, 0.6f);

            imageSet.Y = -200;

            ui.UIImageContainer.AddChild(imageSet);
        }

        private void ExecuteEveryFrames()
        {
            TextWriter.ExecuteEveryFrame();
            ScenarioSceneParts.ForEach(p => p.ExecuteEveryFrame());
        }

        private void LoadSceneResource(Scene next, LoadSceneMode mode)
        {
            Loader loader = new Loader();
            loader.Load(Resource.SceneDirectoryPath);
            GameObject.Find("Logic").GetComponent<ScenarioScene>().Resource = loader.Resource;
            SceneManager.sceneLoaded -= LoadSceneResource;
        }
    }
}