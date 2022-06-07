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
        private ImageSet fillBlackImage;
        private ImageSet mainImageSet = new ImageSet();
        private bool loading;

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
            topBarImage.Y = 350;
            topBarImage.Draw(new List<Sprite>() { imageLoader.LoadImage($@"commonResource\uis\topBar.png") });
            topBarImage.GameObject.GetComponent<SortingGroup>().sortingOrder = 2;

            fillBlackImage = new ImageSet();
            fillBlackImage.Draw(new List<Sprite>() { imageLoader.LoadImage($@"commonResource\uis\fillBlack.png") });
            fillBlackImage.GameObject.GetComponent<SortingGroup>().sortingOrder = 2;
            fillBlackImage.Alpha = 0;

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
                loading = true;
            }

            if (loading)
            {
                fillBlackImage.Alpha += 0.02f;

                if (fillBlackImage.Alpha >= 1.0f)
                {
                    SceneManager.sceneLoaded += LoadNextSceneResource;
                    SceneManager.LoadScene("SampleScene");
                }
            }

            if (mainImageSet.Overwriting)
            {
                mainImageSet.Overwrite(0.01f);
            }
        }

        private void LoadCurrentCursorImage()
        {
            if (Sprites[cursorIndex] == null)
            {
                var firstImagePath = Directory.GetFiles($@"{Paths[cursorIndex]}\images").First();
                Sprites[cursorIndex] = imageLoader.LoadImage(firstImagePath);
            }

            mainImageSet.SetSprite(Sprites[cursorIndex], 0).color = new Color(1, 1, 1, 0);
        }

        private void LoadNextSceneResource(Scene next, LoadSceneMode mode)
        {
            Loader loader = new Loader();
            loader.Load(Paths[cursorIndex]);
            GameObject.Find("Logic").GetComponent<ScenarioScene>().Resource = loader.Resource;
            SceneManager.sceneLoaded -= LoadNextSceneResource;
        }
    }
}
