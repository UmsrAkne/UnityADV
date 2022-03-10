using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{

    private Text textField;
    private int frameCounter = 0;
    private int characterCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            textField.text = "";
            characterCount = 0;
        }

        if (textField.text.Length > 200)
        {
            return;
        }

        if (frameCounter % 3 == 0)
        {
            textField.text += "a";
        }

        if (frameCounter > int.MaxValue - 1)
        {
            frameCounter = 0;
        }

        frameCounter++;
        characterCount++;
    }
}
