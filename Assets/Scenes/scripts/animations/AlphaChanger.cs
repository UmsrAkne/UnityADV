namespace Scenes.Scripts.Animations
{
    using SceneContents;

    public class AlphaChanger : IAnimation
    {
        private IDisplayObject target;
        private float amount = 0.1f;
        private bool canUpdateTarget;

        public AlphaChanger(bool canUpdateTarget = false)
        {
            this.canUpdateTarget = canUpdateTarget;
        }

        public bool IsWorking { get; private set; } = true;

        public double Amount { set => amount = (float)value; }

        public IDisplayObject Target
        {
            get => target;
            set
            {
                if (canUpdateTarget || target == null)
                {
                    target = value;
                }
            }
        }

        public ImageContainer TargetContainer
        {
            set => _ = value;
        }

        public int TargetLayerIndex { get; set; }

        public string AnimationName => nameof(AlphaChanger);

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

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

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            Target.Alpha = 1.0f;
        }
    }
}