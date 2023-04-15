namespace Scenes.Scripts.Animations
{
    using System;
    using UnityEngine;
    using SceneContents;

    public class SlideCore
    {
        private double totalDistance;
        private Vector2 movingDistance = new Vector2(0, 0);
        private double startSectionCount;
        private double finalSectionCount;

        public string AnimationName => "slideCore";

        public int Distance { get; set; }

        public double Speed { get; set; }

        public int Degree { get; set; }

        public bool IsWorking { get; private set; }

        public IDisplayObject Target { private get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            var resistance = 1.0;

            // 開始直後の抵抗値
            if (totalDistance < startSectionCount)
            {
                resistance = startSectionCount <= 0 ? 1.0 : (1.0 / startSectionCount) * totalDistance;
            }

            // 終了直前の抵抗値
            if (totalDistance > finalSectionCount)
            {
                if (finalSectionCount == 0)
                {
                    resistance = 1.0;
                }
                else
                {
                    var distanceOfEnterdFinalSection = totalDistance - finalSectionCount;
                    resistance = 1.0 - (0.01 * distanceOfEnterdFinalSection);
                }
            }

            if (resistance <= 0.1)
            {
                resistance = 0.1;
            }

            var dx = movingDistance.x * Speed * resistance;
            var dy = movingDistance.y * Speed * resistance;

            Target.X += (float)dx;
            Target.Y += (float)dy;

            totalDistance += Speed * resistance;

            if (totalDistance >= Distance)
            {
                Stop();
            }
        }

        public void Start()
        {
            if (Speed > 0 | Distance > 0)
            {
                IsWorking = true;

                var radian = Degree * (Math.PI / 180);
                movingDistance.x = (float)Math.Sin(radian);
                movingDistance.y = (float)Math.Cos(radian);

                startSectionCount = Distance * 0.3;
                finalSectionCount = Distance * 0.8;
            }
        }

        private void Stop()
        {
            IsWorking = false;
            Speed = 0;
            Distance = 0;
        }
    }
}