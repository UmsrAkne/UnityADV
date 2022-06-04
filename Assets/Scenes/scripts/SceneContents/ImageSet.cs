namespace SceneContents
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ImageSet : IDisplayObject
    {
        private float alpha = 1.0f;
        private float scale = 1.0f;
        private int angle = 0;
        private GameObject gameObject = new GameObject("imageSet");
        private GameObject maskObject;
        private List<GameObject> gos = new List<GameObject>();

        public float Alpha
        {
            get => alpha;
            set
            {
                Renderers.ForEach(r => r.color = new Color(1.0f, 1.0f, 1.0f, value));
                alpha = value;
            }
        }

        public double Scale
        {
            get => scale;
            set
            {
                gameObject.transform.localScale = new Vector3((float)value, (float)value, 0);
                scale = (float)value;
            }
        }

        public float X
        {
            get => gameObject.transform.localPosition.x;
            set
            {
                gameObject.transform.localPosition = new Vector3(value, gameObject.transform.localPosition.y);
            }
        }

        public float Y
        {
            get => gameObject.transform.localPosition.y;
            set
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, value);
            }
        }

        public int Angle
        {
            get => angle;
            set
            {
                gameObject.transform.localRotation = Quaternion.AngleAxis((float)value, Vector3.forward);
                angle = value;
            }
        }

        public int SortingLayerIndex { get; set; }

        public List<Sprite> Sprites { get; private set; } = new List<Sprite>();

        public GameObject GameObject => gameObject;

        public GameObject MaskObject => maskObject;

        private List<SpriteRenderer> Renderers { get; set; } = new List<SpriteRenderer>();

        private List<GameObject> GameObjects { get; set; } = new List<GameObject>(4) { null, null, null, null };

        public void Draw()
        {
            var container = this.gameObject;
            var sg = gameObject.AddComponent<SortingGroup>();
            sg.sortingLayerName = $"Layer_{SortingLayerIndex}";

            var gameObjects = new List<GameObject>()
            {
                new GameObject(),
                new GameObject(),
                new GameObject(),
                new GameObject()
            };

            Enumerable.Range(0, Sprites.Count).ToList().ForEach(n =>
            {
                var g = gameObjects[n];
                gos.Add(gameObjects[n]);
                g.transform.SetParent(container.transform, false);
                var renderer = g.AddComponent<SpriteRenderer>();

                renderer.sprite = Sprites[n];

                if (n != 0)
                {
                    g.AddComponent<SpriteMask>().sprite = renderer.sprite;
                }

                Renderers.Add(renderer);
            });

            Renderers[0].sortingOrder = -1;
            Renderers[0].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

            Alpha = alpha;
            Scale = scale;
        }

        public void Draw(List<Sprite> sprites)
        {
            var container = this.gameObject;
            var sg = gameObject.AddComponent<SortingGroup>();
            sg.sortingLayerName = $"Layer_{SortingLayerIndex}";

            for (var i = 0; i < sprites.Count; i++)
            {
                var sp = sprites[i];

                if (sp == null)
                {
                    continue;
                }

                var g = new GameObject();
                GameObjects[i] = g;

                g.transform.SetParent(container.transform, false);
                var r = g.AddComponent<SpriteRenderer>();
                Renderers.Add(r);
                r.sprite = sp;

                if (i == 0)
                {
                    r.sortingOrder = -1;
                    r.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                }
                else
                {
                    g.AddComponent<SpriteMask>().sprite = r.sprite;
                }
            }

            Alpha = alpha;
            Scale = scale;
        }

        public SpriteRenderer SetSprite(Sprite sp)
        {
            var g = new GameObject();
            g.transform.SetParent(this.gameObject.transform, false);
            var renderer = g.AddComponent<SpriteRenderer>();
            renderer.sprite = sp;
            return renderer;
        }

        public void SetMask(Sprite sp)
        {
            if (maskObject == null)
            {
                maskObject = new GameObject("maskObject");
            }

            maskObject.transform.SetParent(GameObject.transform.parent);
            gameObject.transform.SetParent(maskObject.transform);

            var spriteMask = maskObject.GetComponent<SpriteMask>();

            if (spriteMask == null)
            {
                spriteMask = maskObject.AddComponent<SpriteMask>();
            }

            spriteMask.sprite = sp;

            var sg = maskObject.GetComponent<SortingGroup>();

            if (sg == null)
            {
                sg = maskObject.AddComponent<SortingGroup>();
            }

            sg.sortingLayerName = $"Layer_{SortingLayerIndex}";
        }

        /// <summary>
        /// この ImageSet が参照している GameObject を SetActive(false) に設定します。
        /// </summary>
        public void Dispose()
        {
            GameObject?.SetActive(false);
            MaskObject?.SetActive(false);

            gos.ForEach(g => g.SetActive(false));
            GameObjects.ForEach(g => g.SetActive(false));
        }
    }
}