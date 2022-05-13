namespace Animations
{
    public class AlphaChanger : IAnimation
    {
        private ImageSet target;
        private int targetLayerIndex;
        private float amount = 0.1f;

        public bool IsWorking { get; private set; } = true;

        public double Amount { set => amount = (float)value; }

        public ImageSet Target
        {
            get => target;
            set
            {
                if (target == null)
                {
                    target = value;
                }
            }
        }

        public int TargetLayerIndex
        {
            get => targetLayerIndex;
            set => targetLayerIndex = value;
        }

        public string AnimationName => nameof(AlphaChanger);

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            Target.Alpha += amount;

            if (Target.Alpha > 1.0f)
            {
                IsWorking = false;
            }
        }

        public void Stop()
        {
            IsWorking = false;
            Target.Alpha = 1.0f;
        }
    }
}
