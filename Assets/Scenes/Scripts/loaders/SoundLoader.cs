using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scenes.Scripts.SceneContents;
using UnityEngine;
using UnityEngine.Networking;

namespace Scenes.Scripts.Loaders
{
    public class SoundLoader : MonoBehaviour
    {
        public event EventHandler LoadCompleted;

        public List<AudioClip> AudioClips { get; set; } = new List<AudioClip>();

        public List<ISound> AudioSources { get; set; } = new List<ISound>();

        public void Load(List<string> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                if (!string.IsNullOrWhiteSpace(path))
                {
                    StartCoroutine(LoadAudio(path, i));
                }
            }
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
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}