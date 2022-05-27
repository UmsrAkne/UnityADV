namespace SceneParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SceneContents;

    public class SEPlayer : IScenarioSceneParts
    {
        private StopOrder stopOrder;

        public bool NeedExecuteEveryFrame => false;

        private List<ISound> Ses { get; set; }

        private SEOrder CurrentOrder { get; set; }

        private ISound PlayingSound { get; set; }

        public void Execute()
        {
            if (stopOrder != null)
            {
                PlayingSound?.Stop();
                stopOrder = null;
            }

            if (CurrentOrder == null)
            {
                return;
            }

            PlayingSound?.Stop();

            if (Ses.Count > CurrentOrder.Index && CurrentOrder.Index != 0)
            {
                PlayingSound = Ses[CurrentOrder.Index];

                if (CurrentOrder.RepeatCount > 0)
                {
                    PlayingSound.AudioSource.loop = true;
                }

                PlayingSound.Play();
            }

            CurrentOrder = null;
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            Ses = resource.Ses;
        }

        public void SetScenario(Scenario scenario)
        {
            scenario.StopOrders.ForEach(order =>
            {
                if (order.Target == StoppableSceneParts.se)
                {
                    stopOrder = order;
                }
            });

            if (scenario.SEOrders.Count == 0)
            {
                return;
            }

            CurrentOrder = scenario.SEOrders.FirstOrDefault();
        }

        public void SetUI(UI ui)
        {
        }
    }
}
