using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class VoiceLoader : MonoBehaviour
{
    private int loadCompleteCounter;

    public event EventHandler LoadCompleted;

    private event EventHandler PartLoadCompleted;

    public List<ISound> AudioSources { get; private set; } = new List<ISound>();

    private List<AudioClip> AudioClips { get; set; }

    public void Load(string targetDirectoryPath)
    {
        PartLoadCompleted += (sender, e) =>
        {
            loadCompleteCounter++;
            if (loadCompleteCounter >= AudioSources.Count)
            {
                AudioSources.Insert(0, null);
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }
        };

        var audioPaths = GetSoundFilePaths(targetDirectoryPath);
        for (var i = 0; i < audioPaths.Count; i++)
        {
            /// これって GameObject がメモリから消滅しても大丈夫？
            AudioSources.Add(new Sound() { AudioSource = new GameObject().AddComponent<AudioSource>() });
        }

        AudioClips = Enumerable.Repeat<AudioClip>(null, audioPaths.Count).ToList();

        for (var i = 0; i < audioPaths.Count; i++)
        {
            StartCoroutine(LoadAudio(audioPaths[i], i));
        }
    }

    private List<string> GetSoundFilePaths(string targetDirectoryPath)
    {
        var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));

        /// まず ogg ファイルを where で抜き出し、Select でパスを絶対パスに変換する。
        return allFilePaths.Where(f => Path.GetExtension(f) == ".ogg")
                           .Select(p => Path.GetFullPath(p)).ToList();
    }

    private IEnumerator LoadAudio(string path, int index)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
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

            AudioClips[index] = DownloadHandlerAudioClip.GetContent(req);

            if (AudioClips[index].loadState != AudioDataLoadState.Loaded)
            {
                yield break;
            }

            AudioSources[index].AudioSource.clip = AudioClips[index];
            PartLoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
