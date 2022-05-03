using System;
using UnityEngine;

public interface ISound
{
    event EventHandler SoundComplete;

    AudioSource AudioSource { get; set; }

    double Volume { get; set; }

    void Play();

    void Stop();
}
