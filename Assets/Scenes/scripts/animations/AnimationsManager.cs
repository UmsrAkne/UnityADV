using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneContents;
using sceneParts;

namespace animations
{
    public class AnimationsManager : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame => true;

        private List<ImageContainer> ImageContainers { get; set; }

        private List<IAnimation> Animations { get; } = new List<IAnimation>();

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            bool deleteFlag = false;

            Animations.ForEach(anime =>
            {
                anime.Execute();

                if (!anime.IsWorking)
                {
                    deleteFlag = true;
                }
            });

            if (deleteFlag)
            {
                Animations.RemoveAll(anime => !anime.IsWorking);
            }
        }

        public void SetResource(Resource resource)
        {
        }

        public void SetScenario(Scenario scenario)
        {
        }

        public void SetUI(UI ui)
        {
            ImageContainers = ui.ImageContainers;
            ImageContainers.ForEach(imgContainer => { imgContainer.Added += ImageAddedEventHandler; });
        }

        private void ImageAddedEventHandler(object sender, ImageAddedEventArgs e)
        {
            //// 画像が挿入される時、アルファの変化を使ってアニメーションを行う。

            ImageContainer dispatcher = sender as ImageContainer;
            Animations.Add(new AlphaChanger()
            {
                Target = ImageContainers[dispatcher.Index].FrontChild.GetComponent<ImageSet>()
            });
        }
    }
}
