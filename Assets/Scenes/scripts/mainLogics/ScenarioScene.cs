using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioScene : MonoBehaviour
{
    public Resource Resource { private get; set; }

    private List<IScenarioSceneParts> ScenarioSceneParts { get; } = new List<IScenarioSceneParts>();

    private TextWriter TextWriter { get; } = new TextWriter();

    // Start is called before the first frame update
    public void Start()
    {
        ScenarioSceneParts.Add(TextWriter);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ScenarioSceneParts.ForEach(p =>
            {
                p.Execute();
            });
        }

        ScenarioSceneParts.ForEach(p => p.ExecuteEveryFrame());
    }
}
