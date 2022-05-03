using System;
using UnityEngine;

public class Sound : ISound
{
    public event EventHandler SoundComplete;

    public AudioSource AudioSource { get; set; }

    public double Volume { get; set; }

    public void Play()
    {
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }

    public bool IsPlaying()
    {
        if (!AudioSource.isPlaying && AudioSource.time != 0.0f)
        {
            SoundComplete?.Invoke(this, EventArgs.Empty);
        }

        return AudioSource.isPlaying;
    }
}
