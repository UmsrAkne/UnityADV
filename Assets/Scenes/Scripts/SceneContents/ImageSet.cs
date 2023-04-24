using UnityEngine.Rendering;

namespace Scenes.Scripts.SceneContents
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ImageSet : IDisplayObject
    {
        private float alpha = 1.0f;
        private float scale = 1.0f;
        private int angle = 0;
        private ImageUnit parentUnit = new ImageUnit();
        private ImageUnit maskUnit = new ImageUnit();
        private int overwriteLayerIndex = 1;

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

        public SortingGroup SortingGroup => parentUnit.SortingGroup;

        public GameObject GameObject => parentUnit.GameObject;

        public GameObject MaskObject => maskUnit.GameObject;

        public bool Overwriting { get; private set; }

        private List<ImageUnit> ImageUnits { get; set; } = new List<ImageUnit>(4) { null, null, null, null };

        private List<ImageUnit> TemporaryImages { get; set; } = new List<ImageUnit>(4) { null, null, null, null };

        public void Draw(List<SpriteWrapper> spriteWrappers)
        {
            parentUnit.SortingGroup.sortingLayerName = $"Layer_{SortingLayerIndex}";

            for (var i = 0; i < spriteWrappers.Count; i++)
            {
                var spw = spriteWrappers[i];

                if (spw == null)
                {
                    continue;
                }

                var imageUnit = new ImageUnit();
                imageUnit.Width = spw.Width;
                imageUnit.Height = spw.Height;
                ImageUnits[i] = imageUnit;
                imageUnit.SetParent(GameObject);
                imageUnit.SpriteRenderer.sprite = spw.Sprite;

                if (i == 0)
                {
                    imageUnit.SpriteRenderer.sortingOrder = -1;
                    imageUnit.SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                }
                else
                {
                    imageUnit.SetMaskSprite(spw.Sprite);
                }
            }

            Alpha = alpha;
            Scale = scale;
        }

        public SpriteRenderer SetSprite(Sprite sp, int index)
        {
            Overwriting = true;

            if (TemporaryImages[index] != null)
            {
                ReplaceImage(TemporaryImages[index], index);
            }

            var imageUnit = new ImageUnit();
            TemporaryImages[index] = imageUnit;
            imageUnit.SetParent(GameObject);
            imageUnit.SpriteRenderer.sprite = sp;

            // sprite の上書きを常に最前面に対して行う。
            imageUnit.SpriteRenderer.sortingOrder = ++overwriteLayerIndex;

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
                    ReplaceImage(imageUnit, i);
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

        private void ReplaceImage(ImageUnit temporaryImageUnit, int index)
        {
            ImageUnits[index]?.GameObject.SetActive(false);
            ImageUnits[index] = temporaryImageUnit;
            temporaryImageUnit.SpriteRenderer.color = new Color(1, 1, 1, 1);
            TemporaryImages[index] = null;
        }
    }
}