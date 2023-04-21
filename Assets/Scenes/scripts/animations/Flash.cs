namespace Scenes.Scripts.Animations
{
    using System;
    using SceneContents;

    public class Flash : IAnimation
    {
        private ImageContainer targetContainer;
        private int frameCounter;
        private IDisplayObject effectImageSet;

        public string AnimationName => "flash";

        public IDisplayObject EffectImageSet
        {
            get => effectImageSet;
            set => effectImageSet = value;
        }

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public int Duration { get; set; } = 40;

        public int RepeatCount { get; set; } = 1;

        public double Alpha { get; set; } = 1.0f;

        public int Delay { get; set; } = 0;

        public int Interval { get; set; } = 0;

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

            if (Delay-- > 0)
            {
                return;
            }

            frameCounter++;
            effectImageSet.Alpha = GetAlpha();

            if (frameCounter > (Duration + Interval) * RepeatCount)
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