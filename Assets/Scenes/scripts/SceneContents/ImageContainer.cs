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

    public GameObject FrontChild => Childs.FirstOrDefault();

    public GameObject EffectGameObject { get => effectGameObject; }

    public int Index { get; set; }

    private List<GameObject> Childs { get; } = new List<GameObject>();

    public void AddChild(GameObject childObject)
    {
        childObject.transform.SetParent(GameObject.transform);
        Childs.Insert(0, childObject);

        ImageAddedEventArgs e = new ImageAddedEventArgs();
        e.CurrentImageSet = childObject.GetComponent<ImageSet>();
        Added?.Invoke(this, e);
    }

    public void AddEffectLayer()
    {
        if (effectGameObject == null)
        {
            effectGameObject = new GameObject();
            var imageSet = effectGameObject.AddComponent<ImageSet>();

            var loader = new ImageLoader();
            var sp = loader.LoadImage(@"commonResource\uis\fillWhite.png", 1280, 720);
            imageSet.Sprites.Add(sp);
            imageSet.Alpha = 0;

            effectGameObject.transform.SetParent(GameObject.transform);
            Childs.Insert(0, effectGameObject);

            ImageAddedEventArgs e = new ImageAddedEventArgs();
            e.CurrentImageSet = effectGameObject.GetComponent<ImageSet>();

            imageSet.Draw();
            var sortingGroup = effectGameObject.GetComponent<SortingGroup>();
            sortingGroup.sortingOrder = 1;
        }
    }
}
