namespace Scenes.Scripts.SceneContents
{
    using System.Collections.Generic;
    using System.Linq;
    using Scenes.Scripts.Loaders;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ImageContainer
    {
        private GameObject gameObject;
        private GameObject effectGameObject;

        public delegate void ImageAddedEventHandler(object sender, ImageAddedEventArgs e);

        public event ImageAddedEventHandler Added;

        public GameObject GameObject
        {
            get => gameObject;
            set
            {
                if (gameObject == null)
                {
                    gameObject = value;
                }
            }
        }

        public ImageSet FrontChild => Children.FirstOrDefault();

        public GameObject EffectGameObject { get => effectGameObject; }

        public ImageSet EffectImageSet { get; private set; }

        public int Index { get; set; }

        public int Capacity { get; set; } = 4;

        private List<ImageSet> Children { get; } = new List<ImageSet>();

        public void AddChild(ImageSet childObject)
        {
            // childObject.GameObject.name = "children";
            childObject.GameObject.transform.SetParent(GameObject.transform);
            Children.Insert(0, childObject);

            ImageAddedEventArgs e = new ImageAddedEventArgs();
            e.CurrentImageSet = childObject;
            Added?.Invoke(this, e);

            while (Children.Count > Capacity)
            {
                Children.LastOrDefault().Dispose();
                Children.RemoveAt(Children.Count - 1);
            }
        }

        public void AddEffectLayer()
        {
            if (EffectGameObject == null)
            {
                var imageSet = new ImageSet();
                effectGameObject = imageSet.GameObject;
                EffectGameObject.name = "EffectGameObject";

                var loader = new ImageLoader();
                var sp = loader.LoadImage(@"commonResource\uis\fillWhite.png").Sprite;
                imageSet.Alpha = 0;

                EffectGameObject.transform.SetParent(GameObject.transform);

                imageSet.Draw(new List<Sprite>() { sp });
                var sortingGroup = EffectGameObject.GetComponent<SortingGroup>();
                sortingGroup.sortingOrder = 999;

                EffectImageSet = imageSet;
            }
        }
    }
}
