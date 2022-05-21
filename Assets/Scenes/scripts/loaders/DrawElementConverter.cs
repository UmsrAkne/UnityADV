namespace Loaders
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;
    using UnityEngine;

    public class DrawElementConverter : IXMLElementConverter
    {
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

                    order.Names.Add(imageTag.Attribute("a").Value);
                    order.Names.Add(imageTag.Attribute("b").Value);
                    order.Names.Add(imageTag.Attribute("c").Value);
                    order.Names.Add(imageTag.Attribute("d").Value);

                    scenario.DrawOrders.Add(order);
                }
            }
        }
    }
}
