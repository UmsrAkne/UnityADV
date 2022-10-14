namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class DrawElementConverter : IXMLElementConverter
    {
        private List<string> abcdAttribute = new List<string>() { "a", "b", "c", "d" };
        private string depthAttribute = "depth";

        public string TargetElementName => "draw";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement imageTag in tags)
                {
                    var order = new ImageOrder() { IsDrawOrder = true };

                    if (imageTag.Attributes().Any(x => x.Name == "a" || x.Name == "b" || x.Name == "c" || x.Name == "d"))
                    {
                        abcdAttribute.ForEach(s =>
                        {
                            order.Names.Add(imageTag.Attribute(s) != null ? imageTag.Attribute(s).Value : string.Empty);
                        });
                    }

                    if (imageTag.Attribute(depthAttribute) != null)
                    {
                        order.Depth = double.Parse(imageTag.Attribute(depthAttribute).Value);
                    }

                    scenario.DrawOrders.Add(order);
                }
            }
        }
    }
}
