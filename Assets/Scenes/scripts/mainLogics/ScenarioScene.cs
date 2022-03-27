using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioScene : MonoBehaviour
{
    public Resource Resource { private get; set; } = new Resource();

    private List<IScenarioSceneParts> ScenarioSceneParts { get; } = new List<IScenarioSceneParts>();

    private TextWriter TextWriter { get; } = new TextWriter();

    private UI UI { get; } = new UI();

    // Start is called before the first frame update
    public void Start()
    {
        InjectUI(UI);

        ScenarioSceneParts.Add(TextWriter);

        ScenarioSceneParts.Add(new ImageDrawer());

        ScenarioSceneParts.ForEach(s =>
        {
            s.SetResource(Resource);
            s.SetUI(UI);
        });
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
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

    private void InjectUI(UI ui)
    {
        ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_0") });
        ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_1") });
        ui.ImageContainers.Add(new ImageContainer() { GameObject = GameObject.Find("ImageContainer_2") });
    }
}
