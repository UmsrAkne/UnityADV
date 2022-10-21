namespace Scenes.Scripts.Loaders
{
    public class SceneSetting
    {
        public int DefaultImageWidth { get; set; } = 1280;

        public int DefaultImageHeight { get; set; } = 720;

        public int BGMNumber { get; internal set; }

        public float BGMVolume { get; set; } = 1.0f;
    }
}