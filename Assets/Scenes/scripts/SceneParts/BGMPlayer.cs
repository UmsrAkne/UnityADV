using UnityEngine;
using UnityEngine.Audio;

public class BGMPlayer : IScenarioSceneParts
{
    public bool NeedExecuteEveryFrame => false;

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void ExecuteEveryFrame()
    {
        throw new System.NotImplementedException();
    }

    public void SetResource(Resource resource)
    {
        throw new System.NotImplementedException();
    }

    public void SetScenario(Scenario scenario)
    {
        throw new System.NotImplementedException();
    }

    public void SetUI(UI ui)
    {
        throw new System.NotImplementedException();
    }
}
