using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    public List<Sprite> Sprites { get; private set; }

    public void Load(string targetDirectoryPath)
    {
        Sprites = GetImageFileLPaths(Directory.GetCurrentDirectory() + $@"\scenes\{targetDirectoryPath}\images").Select(path =>
        {
            return Sprite.Create(ReadTexture(path, 1280, 720), new Rect(0, 0, 1280, 720), new Vector2(0, 0), 72);
        }).ToList();
    }

    void Start()
    {
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
