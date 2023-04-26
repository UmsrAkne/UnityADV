using System;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.Animations
{
    public class ScaleChange : IAnimation
    {
        private int frameCounter;
        private double changeAmountPerFrame;
        public string AnimationName { get; } = "scaleChange";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public double To { get; set; }

        public int Duration { get; set; }

        public int RepeatCount { get; set; }

        public int Interval { get; set; }

        public int Delay { get; set; }

        public void Execute()
        {
            if (!IsWorking || Target == null)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            CoreProcess();
        }

        private void CoreProcess()
        {
            if (frameCounter == 0)
            {
                changeAmountPerFrame = (Target.Scale - To) / Duration;
            }

            Target.Scale += changeAmountPerFrame;
            frameCounter++;

            if (Math.Abs(Target.Scale - To) <= 0.01)
            {
                Stop();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            changeAmountPerFrame = 0;
            Target.Scale = To;
            frameCounter = Duration + 1;
        }
    }
}