public class AlphaChanger : IAnimation
{
    private ImageSet target;
    private int targetLayerIndex;

    public bool IsWorking { get; private set; } = true;

    public ImageSet Target
    {
        set
        {
            if (target == null)
            {
                target = value;
            }
        }

        get => target;
    }

    public int TargetLayerIndex
    {
        get => targetLayerIndex;
        set => targetLayerIndex = value;
    }

    public void Execute()
    {
        if (!IsWorking)
        {
            return;
        }

        Target.Alpha += 0.01f;

        if (Target.Alpha > 1.0f)
        {
            IsWorking = false;
        }
    }

    public void Stop()
    {
        IsWorking = false;
        Target.Alpha = 1.0f;
    }
}
