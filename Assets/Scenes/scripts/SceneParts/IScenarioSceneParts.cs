public interface IScenarioSceneParts
{
    bool NeedExecuteEveryFrame { get; }

    void Execute();

    void ExecuteEveryFrame();

    void SetScenario(Scenario scenario);

    void SetResource(Resource resource);

    void SetUI();
}
