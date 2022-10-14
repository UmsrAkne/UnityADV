namespace Tests
{
    using System;
    using Scenes.Scripts.SceneContents;
    using UnityEngine;

    /// <summary>
    /// サウンドを再生するクラスをテストするためのダミークラス
    /// </summary>
    public class DummySound : ISound
    {
        private int time;

        public AudioSource AudioSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double Volume { get; set; } = 1.0;

        public bool IsPlaying { get; private set; }

        public int Duration { get; set; }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Forward(int additionTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            time += additionTime;
            if (time >= Duration)
            {
                IsPlaying = false;
            }
        }
    }
}
