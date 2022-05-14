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
        private double resistance = 1.0;
        private int period;
        private bool isInitialExecute = true;

        public string AnimationName => "slide";

        public bool IsWorking { get; private set; } = true;

        public ImageSet Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public double Speed { get; set; } = 1.0;

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

            if (isInitialExecute)
            {
                isInitialExecute = false;
                period = (int)Math.Ceiling(Distance / (Math.Tan(rad) * Speed));
            }

            Target.X += (float)(Math.Sin(rad) * Speed * resistance);
            Target.Y += (float)(Math.Cos(rad) * Speed * resistance);

            totalDistance += Math.Tan(rad) * Speed;

            // 全移動距離に対して、一定割合移動したらブレーキをかける。
            if (totalDistance >= Distance * 0.7)
            {
                resistance = Math.Cos(frameCount * (90.0 / period) * (Math.PI / 180));

                if (resistance <= 0)
                {
                    resistance = 0.01;
                }
            }

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
