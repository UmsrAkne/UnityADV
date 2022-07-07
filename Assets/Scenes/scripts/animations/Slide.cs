namespace Animations
{
    using System;
    using SceneContents;

    public class Slide : IAnimation
    {
        private double totalDistance = 0;
        private bool isInitialExecute = true;
        private IDisplayObject target;
        private SlideCore core;

        public string AnimationName => "slide";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            private get => target;
            set
            {
                if (totalDistance == 0)
                {
                    target = value;
                }
            }
        }

        public int TargetLayerIndex { get; set; }

        public double Speed { get; set; } = 1.0;

        public int Degree { get; set; }

        public int Distance { get; set; }

        public int Duration { get; set; } = int.MaxValue;

        public int LoopCount { get; set; }

        public string Direction
        {
            set
            {
                Enum.TryParse(value, out Direction d);

                // 方向による角度の指定は45度単位とする。
                Degree = (int)d * 45;
            }
        }

        public ImageContainer TargetContainer
        {
            set { _ = value; }
        }

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            if (isInitialExecute)
            {
                isInitialExecute = false;
                core = new SlideCore()
                {
                    Target = Target,
                    Distance = Distance,
                    Degree = Degree,
                    Speed = Speed
                };

                core.Start();
            }

            core.Execute();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            Speed = 0;
            Distance = 0;
        }
    }
}
