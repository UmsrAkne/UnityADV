namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class ImageElementConverter : IXMLElementConverter
    {
        private List<string> abcdAttribute = new List<string>() { "a", "b", "c", "d" };

        public string TargetElementName => "image";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement imageTag in tags)
                {
                    var order = new ImageOrder();

                    if (imageTag.Attributes().Any(x => x.Name == "a" || x.Name == "b" || x.Name == "c" || x.Name == "d"))
                    {
                        abcdAttribute.ForEach(s =>
                        {
                            string name = imageTag.Attribute(s) != null ? imageTag.Attribute(s).Value : string.Empty;
                            order.Names.Add(name);
                        });
                    }
                    else
                    {
                        Log.Add($"image要素に a, b. c, d 属性が含まれていません。Index={scenario.Index}");
                    }

                    if (imageTag.Attribute("scale") != null)
                    {
                        if (double.TryParse(imageTag.Attribute("scale").Value, out double scale))
                        {
                            order.Scale = scale;
                        }
                    }

                    if (imageTag.Attribute("x") != null)
                    {
                        order.X = int.Parse(imageTag.Attribute("x").Value);
                    }

                    if (imageTag.Attribute("y") != null)
                    {
                        order.Y = int.Parse(imageTag.Attribute("y").Value);
                    }

                    if (imageTag.Attribute("angle") != null)
                    {
                        order.Angle = int.Parse(imageTag.Attribute("angle").Value);
                    }

                    if (imageTag.Attribute("mask") != null)
                    {
                        order.MaskImageName = imageTag.Attribute("mask").Value;
                    }

                    scenario.ImageOrders.Add(order);
                }
            }
        }
    }
}