﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ImageLoader
{
    public List<Sprite> Sprites { get; private set; } = new List<Sprite>();

    public Dictionary<string, Sprite> SpriteDictionary { get; private set; } = new Dictionary<string, Sprite>();

    public void Load(string targetDirectoryPath, int imageWidth, int imageHeight)
    {
        GetImageFileLPaths(targetDirectoryPath).ForEach(path =>
        {
            var sp = LoadImage(path, imageWidth, imageHeight);
            Sprites.Add(sp);
            SpriteDictionary.Add(Path.GetFileName(path), sp);
            SpriteDictionary.Add(Path.GetFileNameWithoutExtension(path), sp);
        });
    }

    public Sprite LoadImage(string targetFilePath, int imageWidth, int imageHeight)
    {
        return Sprite.Create(ReadTexture(targetFilePath, imageWidth, imageHeight), new Rect(0, 0, imageWidth, imageHeight), new Vector2(0.5f, 0.5f), 1);
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
}
