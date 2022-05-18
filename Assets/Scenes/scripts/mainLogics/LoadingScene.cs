namespace MainLogics
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Loaders;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class LoadingScene : MonoBehaviour
    {
        private int cursorIndex;
        private bool keyboardLock = true;
        private ImageLoader imageLoader = new ImageLoader();

        private Text Text { get; set; }

        private string[] Paths { get; set; }

        private List<GameObject> GameObjects { get; } = new List<GameObject>();

        private List<Sprite> Sprites { get; } = new List<Sprite>();

        // Start is called before the first frame update
        public void Start()
        {
            Text = GameObject.Find("TextWindow").GetComponent<Text>();
            Paths = Directory.GetDirectories($@"{Directory.GetCurrentDirectory()}\scenes");
            Text.text = Paths[cursorIndex];
            Enumerable.Range(0, Paths.Length).ToList().ForEach(i => GameObjects.Add(new GameObject()));
            Enumerable.Range(0, Paths.Length).ToList().ForEach(i => Sprites.Add(null));
            keyboardLock = false;
            LoadCurrentCursorImage();
        }

        // Update is called once per frame
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) && !keyboardLock)
            {
                if (cursorIndex < Paths.Length - 1)
                {
                    cursorIndex++;
                    Text.text = Paths[cursorIndex];
                    LoadCurrentCursorImage();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && !keyboardLock)
            {
                if (cursorIndex > 0)
                {
                    cursorIndex--;
                    Text.text = Paths[cursorIndex];
                    LoadCurrentCursorImage();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && !keyboardLock)
            {
                SceneManager.sceneLoaded += (Scene next, LoadSceneMode mode) =>
                {
                    Loader loader = new Loader();
                    loader.Load(Paths[cursorIndex]);
                    GameObject.Find("Logic").GetComponent<ScenarioScene>().Resource = loader.Resource;
                };

                SceneManager.LoadScene("SampleScene");
            }
        }

        private void LoadCurrentCursorImage()
        {
            var imageSet = GameObjects[cursorIndex].GetComponent<ImageSet>();

            if (imageSet == null)
            {
                imageSet = GameObjects[cursorIndex].AddComponent<ImageSet>();
            }

            if (Sprites[cursorIndex] == null)
            {
                Sprites[cursorIndex] = imageLoader.LoadImage($@"{Paths[cursorIndex]}\images\A0101.png", 1280, 720);
                imageSet.Sprites.Add(Sprites[cursorIndex]);
                imageSet.Draw();
            }

            GameObjects.ForEach(g =>
            {
                if (g.GetComponent<ImageSet>() != null && g.GetComponent<SortingGroup>() != null)
                {
                    g.GetComponent<ImageSet>().GetComponent<SortingGroup>().sortingOrder = 0;
                }
            });

            imageSet.GetComponent<SortingGroup>().sortingOrder = 1;
        }
    }
}
