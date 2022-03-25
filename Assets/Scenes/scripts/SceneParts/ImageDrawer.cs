using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDrawer : IScenarioSceneParts
{
    private Scenario scenario;
    private Resource resource;

    public bool NeedExecuteEveryFrame => true;

    private List<GameObject> ImageContainers { get; set; } = new List<GameObject>();

    public void Execute()
    {
        // if (scenario.ImageOrders.Count == 0)
        // {
        //     return;
        // }

        // foreach (ImageOrder order in scenario.ImageOrders)
        // {
        //     // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
        //     var targetContainer = ImageContainers[order.TargetLayerIndex];
        //     var emptyGameObject = new GameObject();
        //     emptyGameObject.transform.SetParent(targetContainer.transform);

        //     var imageSet = emptyGameObject.AddComponent<ImageSet>();

        //     // 仮コード
        //     imageSet.Sprites.Add(resource.Images[0]);
        //     imageSet.Sprites.Add(resource.Images[1]);

        //     imageSet.Draw();
        // }


        // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
        var targetContainer = ImageContainers[0];
        var emptyGameObject = new GameObject();
        emptyGameObject.transform.SetParent(targetContainer.transform);

        var imageSet = emptyGameObject.AddComponent<ImageSet>();

        // 仮コード
        imageSet.Sprites.Add(resource.Images[0]);
        imageSet.Sprites.Add(resource.Images[1]);

        imageSet.Draw();
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
        ImageContainers.Add(GameObject.Find("ImageContainer_0"));
        ImageContainers.Add(GameObject.Find("ImageContainer_1"));
        ImageContainers.Add(GameObject.Find("ImageContainer_2"));

        ImageContainers.ForEach(ic =>
        {
            if (ic == null)
            {
                throw new ArgumentNullException("ImageContainers の中に null が含まれています。ゲームオブジェクトの参照要確認");
            }
        });
    }
}
