using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    private SpriteRenderer sr;
    private List<Sprite> sprites;

    void Start()
    {
        gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        sprites = GetImageFileLPaths(Directory.GetCurrentDirectory() + @"\scenes\sampleScn001\images").Select(path =>
        {
            return Sprite.Create(ReadTexture(path, 1280, 720), new Rect(0, 0, 1280, 720), new Vector2(0, 0), 72);
        }).ToList();

        sr.sprite = sprites[0];
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

    private List<string> GetImageFileLPaths(string targetDirectoryPath)
    {
        var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));
        return allFilePaths.Where(f => Path.GetExtension(f) == ".png" || Path.GetExtension(f) == ".jpg").ToList();
    }

    void Update()
    {
    }
}
