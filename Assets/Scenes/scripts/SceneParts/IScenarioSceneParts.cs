public interface IScenarioSceneParts
{
    bool NeedExecuteEveryFrame { get; }

    void execute();

    void executeEveryFrame();

    void setScenario(Scenario scenario);

    void setResource(Resource resource);
}
