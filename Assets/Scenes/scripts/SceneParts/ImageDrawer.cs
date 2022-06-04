﻿namespace SceneParts
{
    using System.Collections.Generic;
    using System.Linq;
    using SceneContents;
    using UnityEngine;

    public class ImageDrawer : IScenarioSceneParts
    {
        private Scenario scenario;
        private Resource resource;

        public bool NeedExecuteEveryFrame => true;

        private List<ImageContainer> ImageContainers { get; set; }

        private List<SpriteRenderer> DrawingImages { get; set; } = new List<SpriteRenderer>();

        public void Execute()
        {
            if (scenario.ImageOrders.Count == 0 && scenario.DrawOrders.Count == 0)
            {
                return;
            }

            foreach (ImageOrder order in scenario.ImageOrders)
            {
                // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
                var targetContainer = ImageContainers[order.TargetLayerIndex];
                var emptyGameObject = new GameObject();
                var imageSet = new ImageSet();

                var sprites = new List<Sprite>();
                order.Names.ForEach(name =>
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        sprites.Add(resource.ImagesByName[name]);
                    }
                    else
                    {
                        sprites.Add(null);
                    }
                });

                imageSet.Alpha = 0;
                imageSet.Scale = order.Scale;
                imageSet.X = order.X;
                imageSet.Y = order.Y;
                imageSet.Angle = order.Angle;
                imageSet.SortingLayerIndex = order.TargetLayerIndex;
                targetContainer.AddChild(imageSet);

                imageSet.Draw(sprites);

                if (!string.IsNullOrWhiteSpace(order.MaskImageName))
                {
                    imageSet.SetMask(resource.MaskImagesByName[order.MaskImageName]);
                }
            }

            foreach (ImageOrder order in scenario.DrawOrders)
            {
                var targetContainer = ImageContainers[order.TargetLayerIndex];
                var frontImageSet = targetContainer.FrontChild;

                for (var i = 0; i < order.Names.Count; i++)
                {
                    var name = order.Names[i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        var r = frontImageSet.SetSprite(resource.ImagesByName[name]);
                        r.color = new Color(1.0f, 1.0f, 1.0f, 0);
                        DrawingImages.Add(r);
                    }
                }
            }
        }

        public void ExecuteEveryFrame()
        {
            bool doDelete = false;

            DrawingImages.ForEach(r =>
            {
                r.color = new Color(1.0f, 1.0f, 1.0f, r.color.a + 0.1f);
                doDelete = r.color.a > 1.0f;
            });

            if (doDelete)
            {
                DrawingImages = DrawingImages.Where(r => r.color.a <= 1.0f).ToList();
            }
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
}
