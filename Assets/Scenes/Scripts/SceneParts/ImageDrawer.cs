using System.Collections.Generic;
using Scenes.Scripts.SceneContents;
using UnityEngine;

namespace Scenes.Scripts.SceneParts
{
    public class ImageDrawer : IScenarioSceneParts
    {
        private Scenario scenario;
        private Resource resource;
        private ImageSet drawingImageSet;
        private double drawingDepth = 0.1;

        public bool NeedExecuteEveryFrame => true;

        private List<ImageContainer> ImageContainers { get; set; }

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
                var imageSet = new ImageSet();

                var spriteWrappers = new List<SpriteWrapper>();
                order.Names.ForEach(name =>
                {
                    spriteWrappers.Add(!string.IsNullOrEmpty(name) ? resource.ImagesByName[name] : null);
                });

                imageSet.Alpha = 0;
                imageSet.Scale = order.Scale;
                imageSet.X = order.X;
                imageSet.Y = order.Y;
                imageSet.Angle = order.Angle;
                imageSet.SortingLayerIndex = order.TargetLayerIndex;
                targetContainer.AddChild(imageSet);

                imageSet.Draw(spriteWrappers);

                if (!string.IsNullOrWhiteSpace(order.MaskImageName))
                {
                    imageSet.SetMask(resource.MaskImagesByName[order.MaskImageName].Sprite);
                }
            }

            foreach (ImageOrder order in scenario.DrawOrders)
            {
                var targetContainer = ImageContainers[order.TargetLayerIndex];
                var frontImageSet = targetContainer.FrontChild;
                drawingImageSet = frontImageSet;
                drawingDepth = order.Depth;

                for (var i = 0; i < order.Names.Count; i++)
                {
                    var name = order.Names[i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        var r = frontImageSet.SetSprite(resource.ImagesByName[name].Sprite, i);
                        r.color = new Color(1.0f, 1.0f, 1.0f, 0);
                    }
                }
            }
        }

        public void ExecuteEveryFrame()
        {
            if (drawingImageSet == null)
            {
                return;
            }

            drawingImageSet.Overwrite((float)drawingDepth);

            if (!drawingImageSet.Overwriting)
            {
                drawingImageSet = null;
            }
        }

        public void SetResource(Resource res)
        {
            this.resource = res;
        }

        public void SetScenario(Scenario scn)
        {
            this.scenario = scn;
        }

        public void SetUI(UI ui)
        {
            ImageContainers = ui.ImageContainers;
        }
    }
}