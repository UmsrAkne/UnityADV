namespace Animations
{
    using System.Drawing;
    using UnityEngine;

    public class Shake : IAnimation
    {
        public string AnimationName => "shake";

        private int TotalMovementDistanceX;
        private int TotalMovementDistanceY;

        public bool IsWorking { get; private set; }

        public ImageSet Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public int Strength { get; set; }

        public int Duration { get; set; }


        public void Execute()
        {
            if (Target == null)
            {
                return;
            }

            Target.X += Strength;
            TotalMovementDistanceY += Strength;

            Duration--;

            if (Duration < 0)
            {
                Stop();
            }
        }

        public void Stop()
        {
            IsWorking = false;
        }
    }
}
