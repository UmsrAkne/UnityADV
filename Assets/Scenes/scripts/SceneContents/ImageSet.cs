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
        private ImageUnit parentUnit = new ImageUnit();
        private ImageUnit maskUnit = new ImageUnit();

        public float Alpha
        {
            get => alpha;
            set
            {
                ImageUnits.ForEach(u =>
                {
                    if (u != null)
                    {
                        u.SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, value);
                    }
                });

                alpha = value;
            }
        }

        public double Scale
        {
            get => scale;
            set
            {
                GameObject.transform.localScale = new Vector3((float)value, (float)value, 0);
                scale = (float)value;
            }
        }

        public float X
        {
            get => GameObject.transform.localPosition.x;
            set
            {
                GameObject.transform.localPosition = new Vector3(value, GameObject.transform.localPosition.y);
            }
        }

        public float Y
        {
            get => GameObject.transform.localPosition.y;
            set
            {
                GameObject.transform.localPosition = new Vector3(GameObject.transform.localPosition.x, value);
            }
        }

        public int Angle
        {
            get => angle;
            set
            {
                GameObject.transform.localRotation = Quaternion.AngleAxis((float)value, Vector3.forward);
                angle = value;
            }
        }

        public int SortingLayerIndex { get; set; }

        public GameObject GameObject => parentUnit.GameObject;

        public GameObject MaskObject => maskUnit.GameObject;

        public bool Overwriting { get; private set; }

        private List<ImageUnit> ImageUnits { get; set; } = new List<ImageUnit>(4) { null, null, null, null };

        private List<ImageUnit> TemporaryImages { get; set; } = new List<ImageUnit>(4) { null, null, null, null };

        public void Draw(List<Sprite> sprites)
        {
            parentUnit.SortingGroup.sortingLayerName = $"Layer_{SortingLayerIndex}";

            for (var i = 0; i < sprites.Count; i++)
            {
                var sp = sprites[i];

                if (sp == null)
                {
                    continue;
                }

                var imageUnit = new ImageUnit();
                ImageUnits[i] = imageUnit;
                imageUnit.SetParent(GameObject);
                imageUnit.SpriteRenderer.sprite = sp;

                if (i == 0)
                {
                    imageUnit.SpriteRenderer.sortingOrder = -1;
                    imageUnit.SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                }
                else
                {
                    imageUnit.SetMaskSprite(sp);
                }
            }

            Alpha = alpha;
            Scale = scale;
        }

        public SpriteRenderer SetSprite(Sprite sp)
        {
            var g = new GameObject();
            g.transform.SetParent(GameObject.transform, false);
            var renderer = g.AddComponent<SpriteRenderer>();
            renderer.sprite = sp;
            return renderer;
        }

        public SpriteRenderer SetSprite(Sprite sp, int index)
        {
            Overwriting = true;
            var imageUnit = new ImageUnit();
            TemporaryImages[index] = imageUnit;
            imageUnit.SetParent(GameObject);
            imageUnit.SpriteRenderer.sprite = sp;
            return imageUnit.SpriteRenderer;
        }

        public void Overwrite(float depth)
        {
            if (!Overwriting)
            {
                return;
            }

            for (var i = 0; i < TemporaryImages.Count; i++)
            {
                var imageUnit = TemporaryImages[i];

                if (imageUnit == null)
                {
                    continue;
                }

                var a = imageUnit.SpriteRenderer.color.a + depth;
                imageUnit.SpriteRenderer.color = new Color(1, 1, 1, a);

                if (a >= 1)
                {
                    ImageUnits[i].GameObject.SetActive(false);
                    ImageUnits[i] = imageUnit;
                    TemporaryImages[i] = null;
                }
            }

            Overwriting = !TemporaryImages.All(i => i == null);
        }

        public void SetMask(Sprite sp)
        {
            maskUnit.SetParent(GameObject.transform.parent.gameObject);
            parentUnit.SetParent(maskUnit.GameObject);
            maskUnit.SetMaskSprite(sp);
            maskUnit.SortingGroup.sortingLayerName = $"Layer_{SortingLayerIndex}";
        }

        /// <summary>
        /// この ImageSet が参照している GameObject を SetActive(false) に設定します。
        /// </summary>
        public void Dispose()
        {
            GameObject?.SetActive(false);
            MaskObject?.SetActive(false);

            ImageUnits.ForEach(u =>
            {
                if (u != null)
                {
                    u.GameObject.SetActive(false);
                }
            });
        }
    }
}