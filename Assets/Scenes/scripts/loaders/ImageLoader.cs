using UnityEngine;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        Texture2D texture = ReadTexture(Directory.GetCurrentDirectory() + @"\graphics\image001.png", 1280, 720);
        Sprite createdSprite = Sprite.Create(texture, new Rect(0, 0, 1280, 720), new Vector2(0, 0), 72);
        sr.sprite = createdSprite;
        sr.transform.position = new Vector3(-9, -5);
    }

    private Texture2D ReadTexture(string path, int width, int height)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        texture.filterMode = FilterMode.Point;
        return texture;
    }

    void Update()
    {
    }
}
