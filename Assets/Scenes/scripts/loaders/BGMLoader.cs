using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class BGMLoader : MonoBehaviour
{
    private AudioClip ac;

    public event EventHandler LoadCompleted;

    public AudioSource AudioSource { get; private set; }

    public void Load(string targetDirectoryPath)
    {
        AudioSource = new GameObject().AddComponent<AudioSource>();
        StartCoroutine(LoadAudio(GetSoundFilePath(targetDirectoryPath)));
    }

    private string GetSoundFilePath(string targetDirectoryPath)
    {
        var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));
        return Path.GetFullPath(allFilePaths.Where(f => Path.GetExtension(f) == ".ogg").FirstOrDefault());
    }

    private IEnumerator LoadAudio(string path)
    {
        if (AudioSource == null || string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            yield break;
        }

        using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS))
        {
            req.SendWebRequest();

            while (!req.isDone)
            {
                yield return null;
            }

            ac = DownloadHandlerAudioClip.GetContent(req);

            if (ac.loadState != AudioDataLoadState.Loaded)
            {
                yield break;
            }

            AudioSource.clip = ac;
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
