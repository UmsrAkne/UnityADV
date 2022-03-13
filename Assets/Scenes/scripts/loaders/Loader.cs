using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{

    Resource resource = new Resource();
    TextLoader textLoader = new TextLoader();

    // Start is called before the first frame update
    void Start()
    {
        textLoader.Load($@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001\texts\scenario.xml");
        resource.Scenarios = textLoader.Scenario;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
