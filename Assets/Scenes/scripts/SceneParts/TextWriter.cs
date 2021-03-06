namespace SceneParts
{
    using System.Collections.Generic;
    using System.Linq;
    using SceneContents;
    using UnityEngine;
    using UnityEngine.UI;

    public class TextWriter : IScenarioSceneParts
    {
        private int counter;
        private bool initialExecute = true;

        public bool Writing { get; private set; }

        public int ScenarioIndex { get; set; }

        public Text Text { get; private set; }

        public string CurrentText { get; set; } = string.Empty;

        public bool NeedExecuteEveryFrame => true;

        private Scenario Scenario { get; set; }

        private List<Scenario> Scenarios { get; set; }

        public void Execute()
        {
            counter = 0;
            if (initialExecute)
            {
                initialExecute = false;
                Scenario = Scenarios.First();
                Writing = true;
                WriteText(string.Empty);
                return;
            }

            if (ScenarioIndex == Scenarios.Count)
            {
                return;
            }

            if (Writing)
            {
                ScenarioIndex++;
                WriteText(Scenario.Text);
                Writing = false;
            }
            else
            {
                Scenario = Scenarios[ScenarioIndex];
                Writing = true;
                WriteText(string.Empty);
            }
        }

        public void ExecuteEveryFrame()
        {
            if (!Writing)
            {
                return;
            }

            AppendText(Scenario.Text[counter]);
            counter++;

            if (Scenario.Text.Length <= counter)
            {
                Writing = false;
                counter = 0;
                ScenarioIndex++;
                return;
            }
        }

        public void SetScenario(Scenario scenario)
        {
            Scenario = scenario;
        }

        public void SetResource(Resource resource)
        {
            Scenarios = resource.Scenarios;
        }

        public void SetText(Text text)
        {
            if (Text == null)
            {
                Text = text;
            }
        }

        public void SetUI(UI ui)
        {
            var component = GameObject.Find("TextField").GetComponent<Text>();

            if (component != null)
            {
                SetText(component);
            }
        }

        public void SetScenarioIndex(int index)
        {
            initialExecute = false;
            ScenarioIndex = index;
            counter = 0;
            WriteText(string.Empty);
            Writing = false;
            Execute();
        }

        private void AppendText(char character)
        {
            if (Text != null)
            {
                Text.text += character;
            }

            CurrentText += character;
        }

        private void WriteText(string str)
        {
            if (Text != null)
            {
                Text.text = str;
            }

            CurrentText = str;
        }
    }
}
