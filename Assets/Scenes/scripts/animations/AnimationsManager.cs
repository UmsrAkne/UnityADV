using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : IScenarioSceneParts
{
    public bool NeedExecuteEveryFrame => true;

    private List<ImageContainer> ImageContainers { get; set; }

    public void Execute()
    {
    }

    public void ExecuteEveryFrame()
    {
    }

    public void SetResource(Resource resource)
    {
    }

    public void SetScenario(Scenario scenario)
    {
    }

    public void SetUI(UI ui)
    {
        ImageContainers = ui.ImageContainers;
    }
}
