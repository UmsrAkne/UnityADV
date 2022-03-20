using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadText : MonoBehaviour
{
    private TextAsset t;

    // Start is called before the first frame update
    public void Start()
    {
        t = Resources.Load("sample01/texts/testtext", typeof(TextAsset)) as TextAsset;
        Debug.Log(t.text);
    }

    // Update is called once per frame
    public void Update()
    {
    }
}
