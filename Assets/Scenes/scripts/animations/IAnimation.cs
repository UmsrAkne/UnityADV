public interface IAnimation
{
    string AnimationName { get; }

    bool IsWorking { get; }

    ImageSet Target { set; }

    ImageContainer TargetContainer { set; }

    int TargetLayerIndex { get; set; }

    void Execute();

    void Stop();
}
