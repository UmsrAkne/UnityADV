using System.Collections.Generic;
using System.Linq;
using SceneContents;
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

    private List<ImageSet> Children { get; } = new List<ImageSet>();

    public void AddChild(ImageSet childObject)
    {
        childObject.GameObject.transform.SetParent(GameObject.transform);
        Children.Insert(0, childObject);

        ImageAddedEventArgs e = new ImageAddedEventArgs();
        e.CurrentImageSet = childObject;
        Added?.Invoke(this, e);
    }

    public void AddEffectLayer()
    {
        if (EffectGameObject == null)
        {
            var imageSet = new ImageSet();
            effectGameObject = imageSet.GameObject;

            var loader = new ImageLoader();
            var sp = loader.LoadImage(@"commonResource\uis\fillWhite.png", 1280, 720);
            imageSet.Sprites.Add(sp);
            imageSet.Alpha = 0;

            EffectGameObject.transform.SetParent(GameObject.transform);
            Children.Insert(0, imageSet);

            imageSet.Draw();
            var sortingGroup = EffectGameObject.GetComponent<SortingGroup>();
            sortingGroup.sortingOrder = 1;

            EffectImageSet = imageSet;
        }
    }
}
