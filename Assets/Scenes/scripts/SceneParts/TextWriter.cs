using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : IScenarioSceneParts
{
    private int counter;
    private bool writing;
    private int scenarioIndex;

    public Text Text { get; private set; }

    public string CurrentText { get; set; } = string.Empty;

    private Scenario Scenario { get; set; }

    private List<Scenario> Scenarios { get; set; }

    public bool NeedExecuteEveryFrame => true;

    public void execute()
    {
        counter = 0;

        if (writing)
        {
            WriteText(Scenario.Text);
            writing = false;
        }
        else
        {
            scenarioIndex++;
            Scenario = Scenarios[scenarioIndex];
            writing = true;
        }
    }

    public void executeEveryFrame()
    {
        if (!writing)
        {
            return;
        }

        AppendText(Scenario.Text[counter]);

        if (Scenario.Text.Length <= counter)
        {
            writing = false;
            counter = 0;
            return;
        }

        counter++;
    }

    public void setScenario(Scenario scenario)
    {
        Scenario = scenario;
    }

    public void setResource(Resource resource)
    {
        Scenarios = resource.Scenarios;
    }

    public void setText(Text text)
    {
        if (Text == null)
        {
            Text = text;
        }
    }

    private void AppendText(char character)
    {
        if (Text != null)
        {
            Text.text += character;
        }

        CurrentText += character;
    }

    private void WriteText(string str)
    {
        if (Text != null)
        {
            Text.text = str;
        }

        CurrentText = str;
    }
}
