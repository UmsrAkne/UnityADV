using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private Resource resource = new Resource();
    private TextLoader textLoader = new TextLoader();
    private ImageLoader imageLoader = new ImageLoader();

    // Start is called before the first frame update
    public void Start()
    {
        var sceneDirectoryPath = $@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001";
        textLoader.Load($@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001\texts\scenario.xml");
        imageLoader.Load($@"{sceneDirectoryPath}\images");
        resource.Scenarios = textLoader.Scenario;
        resource.Images = imageLoader.Sprites;
    }

    // Update is called once per frame
    public void Update()
    {
    }
}
