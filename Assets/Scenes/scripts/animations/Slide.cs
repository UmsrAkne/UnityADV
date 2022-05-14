namespace Animations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class Slide : IAnimation
    {
        private int frameCount;
        private double totalDistance = 0;

        public string AnimationName => "slide";

        public bool IsWorking { get; private set; } = true;

        public ImageSet Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public int Speed { get; set; } = 1;

        public int Degree { get; set; }

        public int Distance { get; set; }

        public int Duration { get; set; } = int.MaxValue;

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            frameCount++;

            var rad = Degree * (Math.PI / 180);

            Target.X += (int)Math.Ceiling(Math.Sin(rad) * Speed);
            Target.Y += (int)Math.Ceiling(Math.Cos(rad) * Speed);

            var t = Math.Tan(rad) * Speed;
            totalDistance += Math.Tan(rad) * Speed;

            if (frameCount >= Duration || Distance < totalDistance)
            {
                Stop();
            }
        }

        public void Stop()
        {
            IsWorking = false;
            frameCount = Duration;
            Speed = 0;
            Distance = 0;
        }
    }
}
