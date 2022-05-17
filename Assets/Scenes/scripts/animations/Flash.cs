namespace Animations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Flash : IAnimation
    {
        public string AnimationName => "flash";

        public bool IsWorking => true;

        public ImageSet Target { set; private get; }

        public int TargetLayerIndex { get; set; }

        public void Execute()
        {
        }

        public void Stop()
        {
            UnityEngine.Debug.Log($"Flash : Execute Stop()");
        }
    }
}
