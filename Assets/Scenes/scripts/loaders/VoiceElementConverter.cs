using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class VoiceElementConverter : IXMLElementConverter
{
    public string TargetElementName => "voice";

    public void Convert(XElement xmlElement, Scenario scenario)
    {
        var tags = xmlElement.Elements(TargetElementName);

        if (tags.Count() != 0)
        {
            foreach (XElement voiceTag in tags)
            {
                var order = new VoiceOrder();

                if (voiceTag.Attribute("number") != null)
                {
                    order.Index = int.Parse(voiceTag.Attribute("number").Value);
                }

                if (voiceTag.Attribute("fileName") != null)
                {
                    order.FileName = voiceTag.Attribute("fileName").Value;
                }

                scenario.VoiceOrders.Add(order);
            }
        }
    }
}
