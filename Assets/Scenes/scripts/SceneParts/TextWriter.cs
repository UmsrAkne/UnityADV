using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : IScenarioSceneParts
{
    public Text Text { get; private set; }

    private Scenario Scenario { get; set; }

    private List<Scenario> Scenarios { get; set; }

    public bool NeedExecuteEveryFrame => true;

    public void execute()
    {
        throw new System.NotImplementedException();
    }

    public void executeEveryFrame()
    {
        throw new System.NotImplementedException();
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
}
