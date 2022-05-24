namespace Animations
{
    using System;
    using SceneContents;

    public class Flash : IAnimation
    {
        private ImageContainer targetContainer;
        private int frameCounter;
        private ImageSet effectImageSet;

        public string AnimationName => "flash";

        public bool IsWorking { get; private set; } = true;

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
            if (!IsWorking)
            {
                return;
            }

            effectImageSet.Alpha = GetAlpha();
            frameCounter++;

            if (frameCounter > Duration * RepeatCount)
            {
                Stop();
            }
        }

        public void Start()
        {
            effectImageSet = TargetContainer.EffectImageSet;
        }

        public void Stop()
        {
            effectImageSet.Alpha = 0;
            IsWorking = false;
        }

        private float GetAlpha()
        {
            var rad = frameCounter * (180 / Duration) * (Math.PI / 180);
            return (float)Math.Abs(Math.Sin(rad)) * (float)Alpha;
        }
    }
}
