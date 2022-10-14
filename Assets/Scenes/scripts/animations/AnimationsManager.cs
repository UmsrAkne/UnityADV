namespace Animations
{
    using System.Collections.Generic;
    using System.Linq;
    using Scenes.Scripts.Animations;
    using Scenes.Scripts.SceneContents;
    using Scenes.Scripts.SceneParts;
    using UnityEngine;

    public class AnimationsManager : IScenarioSceneParts
    {
        private Scenario scenario;

        public AnimationsManager(ImageContainer imageContainer)
        {
            TargetImageContainer = imageContainer;
            TargetImageContainer.Added += ImageAddedEventHandler;
        }

        public bool NeedExecuteEveryFrame => true;

        public ImageContainer TargetImageContainer { get; }

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
            this.scenario = scenario;

            if (scenario.Animations.Count == 0)
            {
                return;
            }

            foreach (var anime in scenario.Animations)
            {
                if (anime.TargetLayerIndex == TargetImageContainer.Index)
                {
                    AddAnimation(anime);
                }
            }
        }

        public void SetUI(UI ui)
        {
        }

        private void ImageAddedEventHandler(object sender, ImageAddedEventArgs e)
        {
            //// 画像が挿入される時、アルファの変化を使ってアニメーションを行う。
            ImageContainer dispatcher = sender as ImageContainer;

            if (!scenario.Animations.Any(a => a.AnimationName == nameof(AlphaChanger) && a.TargetLayerIndex == TargetImageContainer.Index))
            {
                Animations.Add(new AlphaChanger());
            }

            Animations.ForEach(a =>
            {
                a.Target = TargetImageContainer.FrontChild;
            });
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

            anime.TargetContainer = TargetImageContainer;
            anime.Target = TargetImageContainer.FrontChild;
            anime.Start();
            Animations.Add(anime);
        }
    }
}
