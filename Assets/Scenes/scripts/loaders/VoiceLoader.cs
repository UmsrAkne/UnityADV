namespace Scenes.Scripts.Loaders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using SceneContents;
    using UnityEngine;
    using UnityEngine.Networking;

    public class VoiceLoader : MonoBehaviour, IContentsLoader
    {
        private int loadCompleteCounter;

        public event EventHandler LoadCompleted;

        private event EventHandler PartLoadCompleted;

        public List<ISound> AudioSources { get; private set; } = new List<ISound>();

        public Dictionary<string, ISound> AudioSourcesByName { get; private set; } = new Dictionary<string, ISound>();

        public List<string> Log { get; set; } = new List<string>();

        public Resource Resource { get; set; }

        private List<AudioClip> AudioClips { get; set; }

        public TargetAudioType TargetAudioType { get; set; }

        public void Load(string targetDirectoryPath)
        {
            switch (TargetAudioType)
            {
                case TargetAudioType.voice:
                    targetDirectoryPath += $@"\{ResourcePath.SceneVoiceDirectoryName}";
                    break;
                case TargetAudioType.bgVoice:
                    targetDirectoryPath += $@"\{ResourcePath.SceneBgvDirectoryName}";
                    break;
                case TargetAudioType.se:
                    targetDirectoryPath = ResourcePath.CommonSeDirectoryName;
                    break;
            }

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Add($"{targetDirectoryPath} が見つかりませんでした");
                return;
            }

            var audioPaths = GetSoundFilePaths(targetDirectoryPath);

            if (audioPaths.Count == 0)
            {
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }

            PartLoadCompleted += (sender, e) =>
            {
                loadCompleteCounter++;
                if (loadCompleteCounter >= AudioSources.Count)
                {
                    AudioSources.Insert(0, null);

                    switch (TargetAudioType)
                    {
                        case TargetAudioType.voice:
                            Resource.Voices = AudioSources;
                            Resource.VoicesByName = AudioSourcesByName;
                            break;
                        case TargetAudioType.bgVoice:
                            Resource.BGVoices = AudioSources;
                            Resource.BGVoicesByName = AudioSourcesByName;
                            break;
                        case TargetAudioType.se:
                            Resource.Ses = AudioSources;
                            Resource.SesByName = AudioSourcesByName;
                            break;
                    }

                    LoadCompleted?.Invoke(this, EventArgs.Empty);
                }
            };

            for (var i = 0; i < audioPaths.Count; i++)
            {
                /// これって GameObject がメモリから消滅しても大丈夫？
                var sound = new Sound() { AudioSource = new GameObject().AddComponent<AudioSource>() };
                AudioSources.Add(sound);
                AudioSourcesByName.Add(Path.GetFileName(audioPaths[i]), sound);
                AudioSourcesByName.Add(Path.GetFileNameWithoutExtension(audioPaths[i]), sound);
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

    public enum TargetAudioType
    {
        voice,
        bgVoice,
        se,
    }
}