namespace SceneContents
{
    using System.Collections.Generic;
    using Animations;

    public class Scenario
    {
        public int Index { get; set; }

        public string Text { get; set; }

        public List<ImageOrder> ImageOrders { get; set; } = new List<ImageOrder>();

        public List<ImageOrder> DrawOrders { get; set; } = new List<ImageOrder>();

        public List<VoiceOrder> VoiceOrders { get; set; } = new List<VoiceOrder>();

        public int VoiceIndex { get; set; }

        public List<SEOrder> SEOrders { get; set; } = new List<SEOrder>();

        public List<IAnimation> Animations { get; set; } = new List<IAnimation>();
    }
}
