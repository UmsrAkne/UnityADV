namespace Scenes.Scripts.Animations
{
    using System;
    using UnityEngine;
    using SceneContents;

    public class SlideCore : IAnimation
    {
        private double finalSectionCount;
        private Vector2 movingDistance = new Vector2(0, 0);
        private double runningUpDistance;
        private double startSectionCount;
        private double totalDistance;

        public int Distance { get; set; }

        public double Speed { get; set; }

        public int Degree { get; set; }

        public string AnimationName => "slideCore";

        public bool IsWorking { get; private set; }

        public IDisplayObject Target { private get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

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
                if (totalDistance == 0)
                {
                    resistance = 0.2;
                }
                else
                {
                    resistance = GetCustomSinX(totalDistance / startSectionCount * 30.0);
                }
            }

            // 助走に要した距離（最大速度に達するまでの距離）を記録しておく
            if (Math.Abs(resistance - 1.0) < 0.01 && runningUpDistance == 0)
            {
                runningUpDistance = totalDistance;
            }

            // 終了直前の抵抗値
            if (totalDistance > Distance - runningUpDistance)
            {
                var d = totalDistance - (Distance - runningUpDistance);
                resistance = 1.0 - GetCustomSinX(d / (Distance - finalSectionCount) * 30);
            }

            resistance = Math.Max(resistance, 0.2);
            resistance = Math.Min(resistance, 1.0);

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

                // これって sin と cos 逆じゃないですか……? ((('_';)))
                movingDistance.x = (float)Math.Sin(radian);
                movingDistance.y = (float)Math.Cos(radian);

                const double maxAccelerationSectionLength = 100.0;
                startSectionCount = Math.Min(Distance * 0.2, maxAccelerationSectionLength);
                finalSectionCount = Math.Max(Distance * 0.8, Distance - maxAccelerationSectionLength);
            }
        }

        void IAnimation.Stop()
        {
            Stop();
        }

        /// <summary>
        /// 度数をラジアンに変換して、Math.Sinに渡した結果を２倍した値を返す。
        /// </summary>
        /// <param name="deg">パラメーターは度数で入力</param>
        /// <returns>deg=0 のとき 0 を返す。 deg=30 のとき 1.0 を返す。
        /// deg=30 までにゼロから徐々に値を上げていく処理に利用する。
        /// </returns>
        public double GetCustomSinX(double deg)
        {
            return Math.Sin(deg * (Math.PI / 180)) * 2.0;
        }

        private void Stop()
        {
            IsWorking = false;
            Speed = 0;
            Distance = 0;
        }
    }
}