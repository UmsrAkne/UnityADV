﻿namespace Scenes.Scripts.SceneParts
{
    using Scenes.Scripts.SceneContents;

    public interface IScenarioSceneParts
    {
        bool NeedExecuteEveryFrame { get; }

        void Execute();

        void ExecuteEveryFrame();

        void SetScenario(Scenario scenario);

        void SetResource(Resource resource);

        void SetUI(UI ui);
    }
}
