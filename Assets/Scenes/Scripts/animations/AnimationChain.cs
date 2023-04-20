using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.Animations
{
    public class AnimationChain : IAnimation
    {
        private List<IAnimation> animations = new List<IAnimation>();
        private IAnimation playingAnimation;
        private IDisplayObject target;

        public string AnimationName { get; }

        public List<XElement> AnimeTags { get; private set; } = new List<XElement>();

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            private get => target;
            set
            {
                foreach (var a in animations)
                {
                    a.Target = value;
                }

                target = value;
            }
        }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public void AddAnimation(IAnimation anime)
        {
            if (anime is AnimationChain)
            {
                throw new ArgumentException("AnimationChain に AnimationChain は含められません");
            }

            animations.Add(anime);

            if (Target != null)
            {
                anime.Target = Target;
            }
        }

        public void AddAnimationTag(XElement tag)
        {
            AnimeTags.Add(tag);
        }

        public void Execute()
        {
            if (animations.Count == 0 || !IsWorking)
            {
                return;
            }

            if (playingAnimation == null || !playingAnimation.IsWorking)
            {
                playingAnimation = animations.FirstOrDefault(a => a.IsWorking);
            }

            if (playingAnimation == null)
            {
                Stop();
                return;
            }

            playingAnimation.Execute();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            IsWorking = false;
        }
    }
}