using System.Collections.Generic;
using System.Linq;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.SceneParts
{
    public class ChapterManager : IScenarioSceneParts
    {
        private int currentIndex;
        private List<Scenario> scenarios;

        public bool NeedExecuteEveryFrame => false;

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            scenarios = resource.Scenarios;
        }

        public void SetScenario(Scenario scenario)
        {
            currentIndex = scenario.Index;
        }

        public void SetUI(UI ui)
        {
        }

        public int GetNextChapterIndex()
        {
            if (scenarios == null || scenarios.Count() <= currentIndex)
            {
                return currentIndex;
            }

            var nextChapterScenario = scenarios.Skip(currentIndex + 1).FirstOrDefault(scenario => scenario.ChapterName != string.Empty);

            if (nextChapterScenario != null)
            {
                currentIndex = nextChapterScenario.Index - 1;
                return nextChapterScenario.Index - 1;
            }
            else
            {
                return currentIndex;
            }
        }
    }
}
