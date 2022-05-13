namespace Animations
{
    using System.Collections.Generic;
    using System.Linq;
    using SceneContents;
    using SceneParts;
    using UnityEngine;

    public class AnimationsManager : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame => true;

        private List<ImageContainer> ImageContainers { get; set; }

        private List<IAnimation> Animations { get; set; } = new List<IAnimation>();

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
            if (scenario.Animations.Count == 0)
            {
                return;
            }

            foreach (var anime in scenario.Animations)
            {
                AddAnimation(anime);
            }
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
            if (!Animations.Any(a => a.AnimationName == nameof(AlphaChanger)))
            {
                Animations.Add(new AlphaChanger());
            }

            Animations.ForEach(a => a.Target = ImageContainers[dispatcher.Index].FrontChild.GetComponent<ImageSet>());
        }

        /// <summary>
        /// 指定したアニメーションを Animations に追加します。
        /// また、内部に追加するアニメーションと同じものがあった場合、既にある側の Stop() を呼び出します。
        /// </summary>
        private void AddAnimation(IAnimation anime)
        {
            Animations.ForEach(a =>
            {
                if (a.AnimationName == anime.AnimationName)
                {
                    a.Stop();
                }
            });

            Animations.Add(anime);
        }
    }
}
