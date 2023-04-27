﻿namespace Scenes.Scripts.Animations
{
    using System;
    using System.Drawing;
    using SceneContents;

    public class ShakeCore : IAnimation
    {
        private int frameCounter;

        public string AnimationName => "shakeCore";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { private get; set; }

        public ImageContainer TargetContainer { set => _ = value; }

        public int TargetLayerIndex { get; set; }

        public int StrengthX { get; set; }

        public int StrengthY { get; set; }

        public int Duration { get; set; } = 60;

        public int RepeatCount { get; set; }

        // Todo
        public int Delay { get; set; }

        public int Interval { get; set; }

        private Point TotalMovementDistance { get; set; } = new Point(0, 0);

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            double angle = frameCounter * (90.0 / Duration);
            var cos = Math.Cos(angle * (Math.PI / 180));

            int dx = (int)(StrengthX * cos);
            int dy = (int)(StrengthY * cos);

            if (frameCounter != 0)
            {
                // frameCounter < 1 のとき、前回とは逆方向に 2倍 移動するため 2, -2 をかける。
                dx *= (frameCounter % 2 == 0) ? 2 : -2;
                dy *= (frameCounter % 2 == 0) ? 2 : -2;
            }

            Target.X += dx;
            Target.Y += dy;

            TotalMovementDistance = new Point(TotalMovementDistance.X + dx, TotalMovementDistance.Y + dy);

            frameCounter++;

            if (frameCounter >= Duration)
            {
                Stop();
            }
        }

        public void Start()
        {
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