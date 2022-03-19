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

    public void Execute()
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
            Scenario = Scenarios[scenarioIndex - 1];
            writing = true;
            WriteText(string.Empty);
        }
    }

    public void ExecuteEveryFrame()
    {
        if (!writing)
        {
            return;
        }

        AppendText(Scenario.Text[counter]);
        counter++;

        if (Scenario.Text.Length <= counter)
        {
            writing = false;
            counter = 0;
            return;
        }
    }

    public void SetScenario(Scenario scenario)
    {
        Scenario = scenario;
    }

    public void SetResource(Resource resource)
    {
        Scenarios = resource.Scenarios;
    }

    public void SetText(Text text)
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
