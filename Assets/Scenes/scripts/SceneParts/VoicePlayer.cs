using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoicePlayer : IScenarioSceneParts
{
    private int nextPlayIndex;
    private int currentPlayIndex;
    private bool playRequire;
    private ISound currentVoice;
    private VoiceOrder nextOrder;

    public bool NeedExecuteEveryFrame => false;

    public List<ISound> Voices { get; set; }

    public void Execute()
    {
        if (!playRequire)
        {
            return;
        }

        if (currentVoice != null)
        {
            currentVoice.Stop();
        }

        currentVoice = Voices[nextOrder.Index];
        currentVoice.Play();
        currentPlayIndex = nextOrder.Index;
        nextPlayIndex = 0;
        nextOrder = null;
        playRequire = false;
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
        if (scenario.VoiceOrders.Count() == 0)
        {
            return;
        }

        nextOrder = scenario.VoiceOrders.First();

        if (nextOrder.Index >= 0)
        {
            nextPlayIndex = nextOrder.Index;
            playRequire = true;
        }
    }

    public void SetUI(UI ui)
    {
    }
}
