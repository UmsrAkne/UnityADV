﻿namespace Scenes.Scripts.Animations
{
    using System;
    using SceneContents;

    public class Bound : IAnimation
    {
        private int frameCounter;
        private double dx;
        private double dy;

        private double totalDx;
        private double totalDy;

        public string AnimationName => "bound";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        // リフレクション経由でアクセスするため警告を抑制
        // ReSharper disable once MemberCanBePrivate.Global
        public int Duration { get; set; }

        // リフレクション経由でアクセスするため警告を抑制
        // ReSharper disable once MemberCanBePrivate.Global
        public int Degree { get; set; }

        // リフレクション経由でアクセスするため警告を抑制
        // ReSharper disable once MemberCanBePrivate.Global
        public int Strength { get; set; }

        public int RepeatCount { get; set; }

        // Todo
        public int Delay { get; set; }

        public int Interval { get; set; }

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            frameCounter++;

            if (frameCounter == 1)
            {
                dx = Math.Cos(Degree * Math.PI / 180) * Strength;
                dy = Math.Sin(Degree * Math.PI / 180) * Strength;
            }

            if (frameCounter == (Duration / 2) + 1)
            {
                dx *= -1;
                dy *= -1;
            }

            Target.X += (float)dx;
            Target.Y += (float)dy;
            totalDx += (float)dx;
            totalDy += (float)dy;

            if (frameCounter >= Duration)
            {
                if (RepeatCount > 0)
                {
                    frameCounter = 0;
                    RepeatCount--;
                    Target.X -= (float)totalDx;
                    Target.Y -= (float)totalDy;
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            Target.X -= (float)totalDx;
            Target.Y -= (float)totalDy;
            IsWorking = false;
        }
    }
}