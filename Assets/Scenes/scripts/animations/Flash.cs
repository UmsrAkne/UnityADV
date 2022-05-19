namespace Animations
{
    using System;
    using SceneContents;

    public class Flash : IAnimation
    {
        private ImageContainer targetContainer;
        private int frameCounter;

        public string AnimationName => "flash";

        public bool IsWorking => true;

        public IDisplayObject Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public int Cycle { get; set; } = 40;

        public int Duration { get; set; } = 40;

        public int RepeatCount { get; set; } = 1;

        public double Alpha { get; set; } = 1.0f;

        public ImageContainer TargetContainer
        {
            get => targetContainer;
            set
            {
                if (targetContainer == null)
                {
                    targetContainer = value;
                }
            }
        }

        public void Execute()
        {
            if (!IsWorking || TargetContainer == null)
            {
                return;
            }

            var target = TargetContainer.EffectImageSet;

            target.Alpha = GetAlpha();
            frameCounter++;

            if (frameCounter > Duration * RepeatCount)
            {
                Stop();
            }
        }

        public void Stop()
        {
            var target = TargetContainer.EffectImageSet;
            target.Alpha = 0f;
        }

        private float GetAlpha()
        {
            var rad = frameCounter * (180 / Duration) * (Math.PI / 180);
            return (float)Math.Abs(Math.Sin(rad)) * (float)Alpha;
        }
    }
}
