namespace Loaders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;
    using UnityEngine;

    public class SEElementConverter : IXMLElementConverter
    {
        private readonly string numberAttribute = "number";
        private readonly string fileNameAttribute = "fileName";
        private readonly string repeatCountAttribute = "repeatCount";

        public string TargetElementName => "se";

        public List<string> Log => throw new System.NotImplementedException();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement voiceTag in tags)
                {
                    var order = new SEOrder();

                    if (!voiceTag.Attributes().Any(x => x.Name == numberAttribute || x.Name == fileNameAttribute))
                    {
                        throw new FormatException("<se> には fileName か number 属性のどちらかが必須です");
                    }

                    if (voiceTag.Attribute(numberAttribute) != null)
                    {
                        order.Index = int.Parse(voiceTag.Attribute(numberAttribute).Value);
                    }

                    if (voiceTag.Attribute(fileNameAttribute) != null)
                    {
                        order.FileName = voiceTag.Attribute(fileNameAttribute).Value;
                    }

                    if (voiceTag.Attribute(repeatCountAttribute) != null)
                    {
                        order.RepeatCount = int.Parse(voiceTag.Attribute(repeatCountAttribute).Value);
                    }

                    scenario.SEOrders.Add(order);
                }
            }
        }
    }
}
