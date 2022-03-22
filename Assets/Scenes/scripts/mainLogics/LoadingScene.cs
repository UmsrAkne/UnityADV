using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private int cursorIndex;
    private bool keyboardLock = true;

    private Text Text { get; set; }

    private string[] Paths { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find("TextWindow").GetComponent<Text>();
        Paths = Directory.GetDirectories($@"{Directory.GetCurrentDirectory()}\scenes");
        Text.text = Paths[cursorIndex];
        keyboardLock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !keyboardLock)
        {
            if (cursorIndex < Paths.Length - 1)
            {
                cursorIndex++;
                Text.text = Paths[cursorIndex];
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !keyboardLock)
        {
            if (cursorIndex > 0)
            {
                cursorIndex--;
                Text.text = Paths[cursorIndex];
            }
        }
    }
}
