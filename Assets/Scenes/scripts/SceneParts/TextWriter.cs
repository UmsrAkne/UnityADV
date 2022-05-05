using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : IScenarioSceneParts
{
    private int counter;
    private bool writing;
    private bool initialExecute = true;

    public int ScenarioIndex { get; private set; }

    public Text Text { get; private set; }

    public string CurrentText { get; set; } = string.Empty;

    public bool NeedExecuteEveryFrame => true;

    private Scenario Scenario { get; set; }

    private List<Scenario> Scenarios { get; set; }

    public void Execute()
    {
        counter = 0;
        if (initialExecute)
        {
            initialExecute = false;
            Scenario = Scenarios.First();
            writing = true;
            WriteText(string.Empty);
            return;
        }

        if (writing)
        {
            WriteText(Scenario.Text);
            writing = false;
        }
        else
        {
            ScenarioIndex++;
            Scenario = Scenarios[ScenarioIndex];
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

    public void SetUI(UI ui)
    {
        var component = GameObject.Find("TextField").GetComponent<Text>();

        if (component != null)
        {
            SetText(component);
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
