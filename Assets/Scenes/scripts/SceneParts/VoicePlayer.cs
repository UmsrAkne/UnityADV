namespace SceneParts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SceneContents;
    using UnityEngine;

    public class VoicePlayer : IScenarioSceneParts
    {
        private bool playRequire;
        private ISound currentVoice;
        private VoiceOrder nextOrder;

        public event EventHandler SoundComplete;

        public bool NeedExecuteEveryFrame => false;

        public List<ISound> Voices { get; set; }

        public int Channel { get; set; }

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
            nextOrder = null;
            playRequire = false;
        }

        public void ExecuteEveryFrame()
        {
            if (currentVoice != null)
            {
                if (!currentVoice.IsPlaying)
                {
                    SoundComplete?.Invoke(this, EventArgs.Empty);
                    currentVoice = null;
                }
            }
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

            nextOrder = scenario.VoiceOrders.FirstOrDefault(order => order.Channel == Channel);

            // nextOrder.Index == 0 は無視する。
            // [0] は未使用番号。インデックス 0 はデフォルト値であり、未設定の状態を表す。
            if (nextOrder != null && nextOrder.Index > 0)
            {
                playRequire = true;
            }
        }

        public void SetUI(UI ui)
        {
        }
    }
}
