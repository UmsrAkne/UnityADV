﻿using System;
using UnityEngine;

public class Sound : ISound
{
    public AudioSource AudioSource { get; set; }

    public double Volume { get; set; }

    bool ISound.IsPlaying => AudioSource.isPlaying && AudioSource.time != 0f;

    public void Play()
    {
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }
}
