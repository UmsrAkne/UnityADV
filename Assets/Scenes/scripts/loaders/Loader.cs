using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{

    Resource resource = new Resource();
    TextLoader textLoader = new TextLoader();
    ImageLoader imageLoader = new ImageLoader();

    // Start is called before the first frame update
    void Start()
    {
        var sceneDirectoryPath = $@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001";
        textLoader.Load($@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001\texts\scenario.xml");
        imageLoader.Load($@"{sceneDirectoryPath}\images");
        resource.Scenarios = textLoader.Scenario;
        resource.Images = imageLoader.Sprites;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
