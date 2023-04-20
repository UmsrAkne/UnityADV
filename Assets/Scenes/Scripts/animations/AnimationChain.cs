using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Scenes.Scripts.Loaders;
using Scenes.Scripts.SceneContents;

namespace Scenes.Scripts.Animations
{
    public class AnimationChain : IAnimation
    {
        private readonly AnimeElementConverter converter = new AnimeElementConverter();

        private List<IAnimation> animations = new List<IAnimation>();
        private IAnimation playingAnimation;
        private IDisplayObject target;

        public string AnimationName => "AnimationChain";

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
            if (!IsWorking)
            {
                return;
            }

            if (animations.Count(a => a.IsWorking) == 0 && RepeatCount >= 0)
            {
                foreach (var tag in AnimeTags)
                {
                    AddAnimation(converter.GenerateAnimation(tag));
                }

                RepeatCount--;
            }

            if (playingAnimation == null || !playingAnimation.IsWorking)
            {
                playingAnimation = animations.FirstOrDefault(a => a.IsWorking);
                animations = animations.Where(a => a.IsWorking).ToList();
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