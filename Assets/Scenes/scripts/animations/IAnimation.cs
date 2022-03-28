public interface IAnimation
{
    void Execute();

    void Stop();

    bool IsWorking { get; }

    ImageSet Target { set; }

    int TargetLayerIndex { set; }
}
