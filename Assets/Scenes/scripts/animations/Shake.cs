﻿namespace Scenes.Scripts.Animations
{
    using SceneContents;

    public class Shake : IAnimation
    {
        private ShakeCore shakeCore;
        private bool initialExecute = true;
        private int intervalCounter;

        public string AnimationName => "shake";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { private get; set; }

        public ImageContainer TargetContainer { set => _ = value; }

        public int TargetLayerIndex { get; set; }

        public int Strength { get; set; }

        public int Duration { get; set; } = 60;

        public int RepeatCount { get; set; }

        public int Interval { get; set; }

        public void Execute()
        {
            if (Target == null)
            {
                return;
            }

            if (initialExecute)
            {
                initialExecute = false;
                shakeCore = new ShakeCore()
                {
                    Target = Target,
                    Strength = Strength,
                    Duration = Duration,
                };
            }

            shakeCore.Execute();

            if (!shakeCore.IsWorking)
            {
                if (intervalCounter < Interval)
                {
                    intervalCounter++;
                    return;
                }

                if (RepeatCount > 0)
                {
                    RepeatCount--;
                    intervalCounter = 0;
                    initialExecute = true;
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            RepeatCount = 0;
        }
    }
}