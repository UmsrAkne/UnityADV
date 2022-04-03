using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageContainer
{
    private GameObject gameObject;

    public delegate void ImageAddedEventHandler(object sender, ImageAddedEventArgs e);

    public event ImageAddedEventHandler Added = delegate { };

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

    public int Index { get; set; }

    private List<GameObject> Childs { get; } = new List<GameObject>();

    public void AddChild(GameObject childObject)
    {
        childObject.transform.SetParent(GameObject.transform);
        Childs.Insert(0, childObject);

        ImageAddedEventArgs e = new ImageAddedEventArgs();
        e.CurrentImageSet = childObject.GetComponent<ImageSet>();
        Added(this, e);
    }
}
