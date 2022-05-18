namespace MainLogics
{
    using System.Collections.Generic;
    using System.IO;
    using Loaders;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class LoadingScene : MonoBehaviour
    {
        private int cursorIndex;
        private bool keyboardLock = true;
        private ImageLoader imageLoader = new ImageLoader();

        private Text Text { get; set; }

        private string[] Paths { get; set; }

        // Start is called before the first frame update
        public void Start()
        {
            Text = GameObject.Find("TextWindow").GetComponent<Text>();
            Paths = Directory.GetDirectories($@"{Directory.GetCurrentDirectory()}\scenes");
            Text.text = Paths[cursorIndex];
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
            var sp = imageLoader.LoadImage($@"{Paths[cursorIndex]}\images\A0101.png", 1280, 720);
            var emptyGameObject = new GameObject();
            var imageSet = emptyGameObject.AddComponent<ImageSet>();

            imageSet.Sprites.Add(sp);
            imageSet.Draw();
        }
    }
}
