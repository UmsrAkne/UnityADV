using Scenes.Scripts.Animations;
using Scenes.Scripts.SceneContents;

namespace Tests.Animations
{
    public class DummyAnimation : IAnimation
    {
        public int Duration { get; set; }

        /// <summary>
        /// Execute() が実行された回数です。
        /// Execute() の先頭でインクリメントするため、実際の処理が走ったかに関わらずカウントされます。
        /// </summary>
        public int ExecuteCounter { get; private set; }

        /// <summary>
        /// Execute() が実行され、実際に内部で処理が行われた回数です。
        /// ガード節の後でインクリメントされます。
        /// </summary>
        public int ProcessCounter { get; private set; }

        public bool Started { get; set; }

        public bool Stopped { get; set; }

        public string AnimationName => "dummyAnimation";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public void Execute()
        {
            ExecuteCounter++;

            if (!IsWorking)
            {
                return;
            }

            ProcessCounter++;

            if (ProcessCounter >= Duration)
            {
                Stop();
            }
        }

        public void Start()
        {
            Started = true;
        }

        public void Stop()
        {
            IsWorking = false;
            Stopped = true;
        }
    }
}