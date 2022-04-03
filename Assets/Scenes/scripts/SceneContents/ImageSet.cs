using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageSet : MonoBehaviour
{
    private float alpha = 1.0f;
    private float scale = 1.0f;

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
        set
        {
            gameObject.transform.localScale = new Vector3((float)value, (float)value, 0);
            scale = (float)value;
        }
    }

    public List<Sprite> Sprites { get; private set; } = new List<Sprite>();

    private List<SpriteRenderer> Renderers { get; set; } = new List<SpriteRenderer>();

    // Start is called before the first frame update
    public void Start()
    {
    }

    public void Draw()
    {
        var container = this.gameObject;
        gameObject.AddComponent<SortingGroup>();

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

    // Update is called once per frame
    public void Update()
    {
    }
}