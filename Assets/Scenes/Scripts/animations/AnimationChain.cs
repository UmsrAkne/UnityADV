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
        private bool canChangeTarget = true;

        public string AnimationName => "AnimationChain";

        public List<XElement> AnimeTags { get; private set; } = new List<XElement>();

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            private get => target;
            set
            {
                if (!canChangeTarget)
                {
                    // セット不可のタイミングでターゲットの変更が行われた場合、
                    // 外部から画像描画の命令が出ているということであるため、このアニメーションを停止する。
                    Stop();
                    return;
                }

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

        // Todo
        public int Delay { get; set; }

        public int Interval { get; set; }

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

            if (Delay-- > 0)
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

            if (playingAnimation is Draw)
            {
                // 実行アニメーションが Draw の場合は、ターゲット画像の変更が発生する可能性があるため、セッターを許可する。
                canChangeTarget = true;
            }

            playingAnimation.Execute();

            // セッターを許可するのは、Execute() 実行中のみ
            canChangeTarget = false;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            foreach (var a in animations.Where(a => a.IsWorking))
            {
                a.Stop();
            }
        }
    }
}