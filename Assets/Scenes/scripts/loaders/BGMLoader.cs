using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scenes.Scripts.SceneContents;
using UnityEngine;
using UnityEngine.Networking;

namespace Scenes.Scripts.Loaders
{
    public class BGMLoader : MonoBehaviour, IContentsLoader
    {
        private AudioClip ac;

        public event EventHandler LoadCompleted;

        public AudioSource AudioSource { get; private set; }

        public List<string> Log { get; set; } = new List<string>();

        public Resource Resource { get; set; }

        public int BGMNumber { private get; set; }

        public void Load(string targetDirectoryPath)
        {
            if (!Directory.Exists(ResourcePath.CommonBGMDirectoryName))
            {
                Log.Add($"{ResourcePath.CommonBGMDirectoryName} が見つかりませんでした");
                AudioSource = new GameObject().AddComponent<AudioSource>();
                return;
            }

            AudioSource = new GameObject().AddComponent<AudioSource>();
            StartCoroutine(LoadAudio(GetSoundFilePath(ResourcePath.CommonBGMDirectoryName)));
        }

        private string GetSoundFilePath(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));

            if (allFilePaths.Count(p => Path.GetExtension(p) == ".ogg") >= BGMNumber)
            {
                return Path.GetFullPath(allFilePaths.Where(f => Path.GetExtension(f) == ".ogg").ToList()[BGMNumber]);
            }

            return Path.GetFullPath(allFilePaths.FirstOrDefault(f => Path.GetExtension(f) == ".ogg") ?? string.Empty);
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
                Resource.BGMAudioSource = AudioSource;
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}