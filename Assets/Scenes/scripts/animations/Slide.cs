﻿namespace Scenes.Scripts.Animations
{
    using System;
    using SceneContents;

    public class Slide : IAnimation
    {
        private int executeCounter;
        private bool isInitialExecute = true;
        private IDisplayObject target;
        private SlideCore core;
        private int stopTimeCount;

        public string AnimationName => "slide";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            private get => target;
            set
            {
                // アニメーションの挿入 -> 直後に画像切替　の間に数回は実行されてしまう可能性があるため
                // 実行 2 回程度はまだ実行していないものとしてターゲットの変更を許可する
                if (executeCounter < 3)
                {
                    target = value;
                }
                else
                {
                    Stop();
                }
            }
        }

        public int TargetLayerIndex { get; set; }

        public double Speed { get; set; } = 1.0;

        public int Degree { get; set; }

        public int Distance { get; set; }

        public int Duration { get; set; } = int.MaxValue;

        public int RepeatCount { get; set; }

        // Todo
        public int Delay { get; set; }

        public int Interval { get; set; }

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
            set => _ = value;
        }

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            if (isInitialExecute)
            {
                isInitialExecute = false;
                Initialize();
            }

            core.Execute();
            executeCounter++;

            if (!core.IsWorking)
            {
                stopTimeCount++;

                if (Interval > stopTimeCount)
                {
                    return;
                }

                stopTimeCount = 0;

                if (RepeatCount > 0)
                {
                    RepeatCount--;
                    Degree += 180;
                    Initialize();
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
            IsWorking = false;
            Speed = 0;
            Distance = 0;
        }

        private void Initialize()
        {
            core = new SlideCore()
            {
                Target = Target,
                Distance = Distance,
                Degree = Degree,
                Speed = Speed
            };

            core.Start();
        }
    }
}