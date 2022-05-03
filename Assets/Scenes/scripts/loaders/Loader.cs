using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader
{
    private TextLoader textLoader = new TextLoader();
    private ImageLoader imageLoader = new ImageLoader();
    private BGMLoader bgmLoader = new GameObject().AddComponent<BGMLoader>();
    private VoiceLoader voiceLoader = new GameObject().AddComponent<VoiceLoader>();

    public Resource Resource { get; set; } = new Resource();

    public void Load(string path)
    {
        textLoader.Load($@"{path}\texts\scenario.xml");
        imageLoader.Load($@"{path}\images");
        voiceLoader.Load($@"{path}\voices");
        bgmLoader.Load(@"commonResource\bgms");

        Resource.Scenarios = textLoader.Scenario;
        Resource.Images = imageLoader.Sprites;
        Resource.ImagesByName = imageLoader.SpriteDictionary;
        Resource.BGMAudioSource = bgmLoader.AudioSource;
        Resource.Voices = voiceLoader.AudioSources;
    }
}
