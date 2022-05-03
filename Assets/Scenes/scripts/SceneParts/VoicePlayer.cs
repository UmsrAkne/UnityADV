using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePlayer : IScenarioSceneParts
{
    private int nextPlayIndex;
    private int currentPlayIndex;
    private bool playRequire;

    public bool NeedExecuteEveryFrame => false;

    public List<ISound> Voices { get; set; }

    public void Execute()
    {
        if (!playRequire)
        {
            return;
        }

        Voices[nextPlayIndex].Play();
        currentPlayIndex = nextPlayIndex;
        nextPlayIndex = 0;
    }

    public void ExecuteEveryFrame()
    {
    }

    public void SetResource(Resource resource)
    {
        Voices = resource.Voices;
    }

    public void SetScenario(Scenario scenario)
    {
        if (scenario.VoiceIndex != 0)
        {
            nextPlayIndex = scenario.VoiceIndex;
            playRequire = true;
        }
    }

    public void SetUI(UI ui)
    {
    }
}
