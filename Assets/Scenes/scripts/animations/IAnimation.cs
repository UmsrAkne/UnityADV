namespace Animations
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

        void Execute();

        void Start();

        void Stop();
    }
}
