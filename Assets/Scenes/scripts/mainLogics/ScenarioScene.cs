using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioScene : MonoBehaviour
{
    public Resource Resource { private get; set; } = new Resource();

    private List<IScenarioSceneParts> ScenarioSceneParts { get; } = new List<IScenarioSceneParts>();

    private TextWriter TextWriter { get; } = new TextWriter();

    // Start is called before the first frame update
    public void Start()
    {
        ScenarioSceneParts.Add(TextWriter);

        ScenarioSceneParts.Add(new ImageDrawer());

        ScenarioSceneParts.ForEach(s =>
        {
            s.SetResource(Resource);
            s.SetUI();
        });
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Forward();
        }

        ScenarioSceneParts.ForEach(p => p.ExecuteEveryFrame());
    }

    public void Forward()
    {
        ScenarioSceneParts.ForEach(p =>
        {
            p.SetScenario(Resource.Scenarios[TextWriter.ScenarioIndex]);
            p.Execute();
        });
    }
}
