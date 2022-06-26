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

        private List<ImageSet> miniImageSets = new List<ImageSet>();

        private bool loading;
        private string lastSelectedSceneIndexKey = "lastSelectedSceneIndex";

        private Text Text { get; set; }

        private string[] Paths { get; set; }

        private List<SpriteWrapper> Sprites { get; } = new List<SpriteWrapper>();

        // Start is called before the first frame update
        public void Start()
        {
            if (PlayerPrefs.HasKey(lastSelectedSceneIndexKey))
            {
                cursorIndex = PlayerPrefs.GetInt(lastSelectedSceneIndexKey);
            }

            Text = GameObject.Find("TextWindow").GetComponent<Text>();
            Paths = Directory.GetDirectories($@"{Directory.GetCurrentDirectory()}\scenes");
            Text.text = Paths[cursorIndex];

            var topBarImage = new ImageSet();
            topBarImage.Y = 350;
            topBarImage.Draw(new List<Sprite>() { imageLoader.LoadImage($@"commonResource\uis\topBar.png").Sprite });
            topBarImage.GameObject.GetComponent<SortingGroup>().sortingOrder = 2;

            fillBlackImage = new ImageSet();
            fillBlackImage.Draw(new List<Sprite>() { imageLoader.LoadImage($@"commonResource\uis\fillBlack.png").Sprite });
            fillBlackImage.GameObject.GetComponent<SortingGroup>().sortingOrder = 2;
            fillBlackImage.Alpha = 0;

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
                Text.color = new Color(1, 1, 1, Text.color.a - 0.05f);

                if (fillBlackImage.Alpha >= 1.0f)
                {
                    PlayerPrefs.SetInt(lastSelectedSceneIndexKey, cursorIndex);
                    SceneManager.sceneLoaded += LoadNextSceneResource;
                    SceneManager.LoadScene("SampleScene");
                }
            }

            if (mainImageSet.Overwriting)
            {
                mainImageSet.Overwrite(0.1f);
            }
        }

        private void LoadCurrentCursorImage()
        {
            if (Sprites[cursorIndex] == null)
            {
                var firstImagePath = Directory.GetFiles($@"{Paths[cursorIndex]}\images").First();
                Sprites[cursorIndex] = imageLoader.LoadImage(firstImagePath);
            }

            mainImageSet.SetSprite(Sprites[cursorIndex].Sprite, 0).color = new Color(1, 1, 1, 0);
            mainImageSet.X = 320;

            for (var i = 0; i < 4; i++)
            {
                var index = cursorIndex + i;

                if (index >= Sprites.Count)
                {
                    return;
                }

                if (Sprites[index] == null)
                {
                    Sprites[index] = imageLoader.LoadImage(Directory.GetFiles($@"{Paths[index]}\images").First());
                }

                var miniImage = new ImageSet();
                miniImage.SetSprite(Sprites[index].Sprite, 0);
                miniImage.Scale = 320.0 / Sprites[index].Width;
                miniImage.X = -480;
                miniImage.Y = 270 + (-180 * i);
            }
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
