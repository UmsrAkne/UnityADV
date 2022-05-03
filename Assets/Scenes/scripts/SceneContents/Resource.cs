using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public List<Scenario> Scenarios { get; set; }

    public List<Sprite> Images { get; set; }

    public Dictionary<string, Sprite> ImagesByName { get; set; }

    public AudioSource BGMAudioSource { get; set; }

    public List<AudioSource> Voices { get; set; }
}
