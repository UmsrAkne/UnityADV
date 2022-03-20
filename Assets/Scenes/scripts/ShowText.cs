using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    private Text textField;
    private int frameCounter;
    private int characterCount;
    private int position;
    private bool writing;
    private string currentText = string.Empty;

    public List<string> Texts { get; set; } = new List<string>() { };

    // Start is called before the first frame update
    public void Start()
    {
        textField = GetComponent<Text>();
        textField.text = string.Empty;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToNext();
            return;
        }

        if (frameCounter % 10 == 0 && writing)
        {
            textField.text += currentText[characterCount];
            characterCount++;
            frameCounter = 0;

            if (characterCount >= currentText.Length)
            {
                WritingComplete();
            }
        }

        frameCounter++;
    }

    private void ToNext()
    {
        if (position >= Texts.Count)
        {
            return;
        }

        currentText = Texts[position];

        if (writing)
        {
            textField.text = currentText;
            WritingComplete();
        }
        else
        {
            textField.text = string.Empty;
            characterCount = 0;
            writing = true;
        }
    }

    private void WritingComplete()
    {
        position++;
        writing = false;
    }
}
