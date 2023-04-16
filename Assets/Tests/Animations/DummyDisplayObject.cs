using Scenes.Scripts.SceneContents;

namespace Tests.Animations
{
    public class DummyDisplayObject : IDisplayObject
    {
        public float Alpha { get; set; } = 1.0f;

        public double Scale { get; set; } = 1.0;

        public float X { get; set; } = 0;

        public float Y { get; set; } = 0;

        public int Angle { get; set; } = 0;
    }
}