namespace Scenes.Scripts.Animations
{
    using SceneContents;

    public interface IAnimation
    {
        string AnimationName { get; }

        bool IsWorking { get; }

        IDisplayObject Target { set; }

        ImageContainer TargetContainer { set; }

        int TargetLayerIndex { get; set; }

        int RepeatCount { get; set; }

        int Delay { get; set; }

        int Interval { get; set; }

        void Execute();

        void Start();

        void Stop();
    }
}