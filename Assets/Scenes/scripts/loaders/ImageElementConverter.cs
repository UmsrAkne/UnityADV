using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ImageElementConverter : IXMLElementConverter
{
    public string TargetElementName => "image";

    public void Convert(XElement xmlElement, Scenario scenario)
    {
        var tags = xmlElement.Elements(TargetElementName);

        if (tags.Count() != 0)
        {
            foreach (XElement imageTag in tags)
            {
                var order = new ImageOrder();

                order.Names.Add(imageTag.Attribute("a").Value);
                order.Names.Add(imageTag.Attribute("b").Value);
                order.Names.Add(imageTag.Attribute("c").Value);
                order.Names.Add(imageTag.Attribute("d").Value);

                if (imageTag.Attribute("scale") != null)
                {
                    if (double.TryParse(imageTag.Attribute("scale").Value, out double scale))
                    {
                        order.Scale = scale;
                    }
                }

                scenario.ImageOrders.Add(order);
            }
        }
    }
}
