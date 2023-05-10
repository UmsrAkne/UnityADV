using System;
using System.Collections.Concurrent;
using System.Linq;
using Scenes.Scripts.SceneContents;
using Scenes.Scripts.SceneParts;

namespace Scenes.Scripts.Animations
{
    public class AnimationsManager : IScenarioSceneParts
    {
        private Scenario scn;

        public AnimationsManager(ImageContainer imageContainer)
        {
            TargetImageContainer = imageContainer;
            TargetImageContainer.Added += ImageAddedEventHandler;
        }

        public bool NeedExecuteEveryFrame => true;

        public ImageContainer TargetImageContainer { get; }

        private ConcurrentBag<IAnimation> Animations { get; set; } = new ConcurrentBag<IAnimation>();

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            bool deleteFlag = false;

            foreach (var a in Animations)
            {
                a.Execute();

                if (!a.IsWorking)
                {
                    deleteFlag = true;
                }
            }

            if (deleteFlag)
            {
                Animations = new ConcurrentBag<IAnimation>(Animations.Where(anime => anime.IsWorking));
            }
        }

        public void SetResource(Resource resource)
        {
        }

        public void SetScenario(Scenario scenario)
        {
            scn = scenario;

            if (scenario.Animations.Count == 0 && scenario.StopOrders.All(s => !s.IsAnimationStopOrder()))
            {
                return;
            }

            foreach (var s in scenario.StopOrders.Where(so => so.IsAnimationStopOrder()))
            {
                foreach (var a in Animations)
                {
                    if (a.AnimationName.Equals(s.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        a.Stop();
                    }
                }
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
            if (!scn.Animations.Any(a => a.AnimationName == nameof(AlphaChanger) && a.TargetLayerIndex == TargetImageContainer.Index))
            {
                Animations.Add(new AlphaChanger());
            }

            foreach (var a in Animations)
            {
                a.Target = TargetImageContainer.FrontChild;
            }
        }

        /// <summary>
        /// 指定したアニメーションを Animations に追加します。
        /// また、内部に追加するアニメーションと同じものがあった場合、既にある側の Stop() を呼び出します。
        /// </summary>
        private void AddAnimation(IAnimation anime)
        {
            foreach (var a in Animations)
            {
                if (a.AnimationName == anime.AnimationName)
                {
                    a.Stop();
                }
            }

            anime.TargetContainer = TargetImageContainer;
            anime.Target = TargetImageContainer.FrontChild;
            anime.Start();
            Animations.Add(anime);
        }
    }
}