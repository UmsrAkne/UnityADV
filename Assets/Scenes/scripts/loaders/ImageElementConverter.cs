using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
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

                scenario.ImageOrders.Add(order);
            }
        }
    }
}
