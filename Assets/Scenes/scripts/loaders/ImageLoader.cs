using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ImageLoader
{
    public List<Sprite> Sprites { get; private set; } = new List<Sprite>();

    public Dictionary<string, Sprite> SpriteDictionary { get; private set; } = new Dictionary<string, Sprite>();

    public void Load(string targetDirectoryPath)
    {
        GetImageFileLPaths(targetDirectoryPath).ForEach(path =>
        {
            var sp = LoadImage(path);
            Sprites.Add(sp);
            SpriteDictionary.Add(Path.GetFileName(path), sp);
            SpriteDictionary.Add(Path.GetFileNameWithoutExtension(path), sp);
        });
    }

    public Sprite LoadImage(string targetFilePath)
    {
        var size = GetImageSize(targetFilePath);
        return Sprite.Create(ReadTexture(targetFilePath, (int)size.x, (int)size.y), new Rect(0, 0, (int)size.x, (int)size.y), new Vector2(0.5f, 0.5f), 1);
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

    private Vector2 GetImageSize(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        fs.Seek(16, SeekOrigin.Begin);
        byte[] buf = new byte[8];
        fs.Read(buf, 0, 8);
        fs.Dispose();
        uint width = ((uint)buf[0] << 24) | ((uint)buf[1] << 16) | ((uint)buf[2] << 8) | (uint)buf[3];
        uint height = ((uint)buf[4] << 24) | ((uint)buf[5] << 16) | ((uint)buf[6] << 8) | (uint)buf[7];
        return new Vector2(width, height);
    }
}
