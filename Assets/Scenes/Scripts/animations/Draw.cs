using System.Collections.Generic;
using Scenes.Scripts.SceneContents;
using Scenes.Scripts.SceneParts;

namespace Scenes.Scripts.Animations
{
    public class Draw : IAnimation
    {
        public string AnimationName { get; } = "draw";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public double Scale { get; set; }

        public string A { get; set; } = string.Empty;

        public string B { get; set; } = string.Empty;

        public string C { get; set; } = string.Empty;

        public string D { get; set; } = string.Empty;

        public static ImageDrawer ImageDrawer { private get; set; }

        public void Execute()
        {
            if (ImageDrawer == null || !IsWorking)
            {
                return;
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
        }
    }
}