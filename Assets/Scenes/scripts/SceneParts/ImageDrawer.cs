using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDrawer : IScenarioSceneParts
{
    private Scenario scenario;
    private Resource resource;

    public bool NeedExecuteEveryFrame => true;

    private List<GameObject> ImageContainers { get; set; }

    public void Execute()
    {
        if (scenario.ImageOrders.Count == 0)
        {
            return;
        }
    }

    public void ExecuteEveryFrame()
    {
    }

    public void SetResource(Resource resource)
    {
        this.resource = resource;
    }

    public void SetScenario(Scenario scenario)
    {
        this.scenario = scenario;
    }

    public void SetUI()
    {
        ImageContainers.Add(GameObject.Find("ImageContainer-0"));
        ImageContainers.Add(GameObject.Find("ImageContainer-1"));
        ImageContainers.Add(GameObject.Find("ImageContainer-2"));

        ImageContainers.ForEach(ic =>
        {
            if (ic == null)
            {
                throw new ArgumentNullException("ImageContainers の中に null が含まれています。ゲームオブジェクトの参照要確認");
            }
        });
    }
}
