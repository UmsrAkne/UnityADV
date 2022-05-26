namespace SceneContents
{
    using System.Collections.Generic;

    public class BGVOrder
    {
        public List<string> FileNames { get; set; } = new List<string>();

        public int Channel { get; set; }

        public bool IsStopOrder { get; set; }
    }
}
