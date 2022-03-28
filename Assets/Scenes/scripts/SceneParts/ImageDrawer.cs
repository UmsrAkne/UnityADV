using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDrawer : IScenarioSceneParts
{
    private Scenario scenario;
    private Resource resource;

    public bool NeedExecuteEveryFrame => true;

    private List<ImageContainer> ImageContainers { get; set; }

    public void Execute()
    {
        if (scenario.ImageOrders.Count == 0)
        {
            return;
        }

        foreach (ImageOrder order in scenario.ImageOrders)
        {
            // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
            var targetContainer = ImageContainers[order.TargetLayerIndex];
            var emptyGameObject = new GameObject();
            var imageSet = emptyGameObject.AddComponent<ImageSet>();

            order.Names.ForEach(name =>
            {
                if (!string.IsNullOrEmpty(name))
                {
                    imageSet.Sprites.Add(resource.ImagesByName[name]);
                }
            });

            targetContainer.AddChild(emptyGameObject);

            imageSet.Draw();
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

    public void SetUI(UI ui)
    {
        ImageContainers = ui.ImageContainers;
    }
}
