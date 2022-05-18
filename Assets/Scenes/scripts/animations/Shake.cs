namespace Animations
{
    using System;
    using System.Drawing;
    using SceneContents;

    public class Shake : IAnimation
    {
        private int frameCounter;

        public string AnimationName => "shake";

        public bool IsWorking { get; private set; } = true;

        public ImageSet Target { private get; set; }

        public ImageContainer TargetContainer
        {
            set { _ = value; }
        }

        public int TargetLayerIndex { get; set; }

        public int Strength { get; set; }

        public int Duration { get; set; } = 60;

        private Point TotalMovementDistance { get; set; } = new Point(0, 0);

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            double angle = frameCounter * (90.0 / Duration);
            var cos = Math.Cos(angle * (Math.PI / 180));

            int strength = (int)(Strength * cos);

            if (frameCounter != 0)
            {
                strength *= (frameCounter % 2 == 0) ? 2 : -2;
            }

            Target.X += strength;
            Target.Y += strength;

            TotalMovementDistance = new Point(TotalMovementDistance.X + strength, TotalMovementDistance.Y + strength);

            frameCounter++;

            if (frameCounter >= Duration)
            {
                Stop();
            }
        }

        public void Stop()
        {
            IsWorking = false;
            frameCounter = Duration;
            Target.X -= TotalMovementDistance.X;
            Target.Y -= TotalMovementDistance.Y;
        }
    }
}
