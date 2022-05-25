namespace SceneContents
{
    using System.Collections.Generic;

    public class BGVOrder
    {
        public List<string> FileNames { get; set; } = new List<string>();

        public bool IsStopOrder { get; set; }
    }
}
