namespace SceneParts
{
    using System;
    using System.Collections.Generic;
    using SceneContents;

    public class BGVoicePlayer : IScenarioSceneParts
    {
        private BGVOrder currentOrder;
        private bool playRequest;
        private VoicePlayer voicePlayer;
        private int channel;
        private List<ISound> bgVoices;
        private Dictionary<string, ISound> bgVoicesByName;

        public BGVoicePlayer(VoicePlayer voicePlayer)
        {
            this.voicePlayer = voicePlayer;
            channel = voicePlayer.Channel;
        }

        public bool NeedExecuteEveryFrame => true;

        public void Execute()
        {
            if (!playRequest)
            {
                return;
            }
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            bgVoices = resource.BGVoices;
            bgVoicesByName = resource.BGVoicesByName;
        }

        public void SetScenario(Scenario scenario)
        {
            scenario.BGVOrders.ForEach(order =>
            {
                if (order.Channel == channel)
                {
                    currentOrder = order;
                    playRequest = true;
                }
            });
        }

        public void SetUI(UI ui)
        {
        }
    }
}
