using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{

    TextLoader textLoader = new TextLoader();

    // Start is called before the first frame update
    void Start()
    {
        textLoader.Load($@"{Directory.GetCurrentDirectory()}\scenes\sampleScn001\texts\scenario.xml");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
