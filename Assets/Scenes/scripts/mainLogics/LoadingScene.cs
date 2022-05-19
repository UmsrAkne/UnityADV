namespace MainLogics
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Loaders;
    using SceneContents;
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

            var topBarImage = new ImageSet();
            topBarImage.Sprites.Add(imageLoader.LoadImage($@"commonResource\uis\topBar.png", 1280, 50));
            topBarImage.Y = 350;
            topBarImage.Draw();
            topBarImage.GameObject.GetComponent<SortingGroup>().sortingOrder = 2;

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
            var imageSet = new ImageSet();

            if (imageSet == null)
            {
                imageSet = new ImageSet();
            }

            if (Sprites[cursorIndex] == null)
            {
                var fistImagePath = Directory.GetFiles($@"{Paths[cursorIndex]}\images").First();
                Sprites[cursorIndex] = imageLoader.LoadImage(fistImagePath, 1280, 720);
                imageSet.Sprites.Add(Sprites[cursorIndex]);
                imageSet.Draw();
            }

            GameObjects.ForEach(g =>
            {
                // if (g.GetComponent<ImageSet>() != null && g.GetComponent<SortingGroup>() != null)
                // {
                //     // g.GetComponent<ImageSet>().GetComponent<SortingGroup>().sortingOrder = 0;
                // }
            });

            // imageSet.GetComponent<SortingGroup>().sortingOrder = 1;
        }
    }
}
