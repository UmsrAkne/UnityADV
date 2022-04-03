public interface IAnimation
{
    bool IsWorking { get; }

    ImageSet Target { set; }

    int TargetLayerIndex { get; set; }

    void Execute();

    void Stop();
}
