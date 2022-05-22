namespace Animations
{
    using System;
    using SceneContents;

    public class Bound : IAnimation
    {
        private int frameCounter;

        public string AnimationName => "bound";

        public bool IsWorking => true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int Duration { get; set; }

        public int Degree { get; set; }

        public int Strength { get; set; }

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

        }

        public void Stop()
        {
        }
    }
}
