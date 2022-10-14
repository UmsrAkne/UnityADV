namespace Scenes.Scripts.Loaders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using SceneContents;

    public class BackgroundVoiceElementConverter : IXMLElementConverter
    {
        private string namesAttribute = "names";
        private string channelAttribute = "channel";

        public string TargetElementName => "backgroundVoice";

        public List<string> Log => new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            if (tags.Count() != 0)
            {
                foreach (XElement bgvTag in tags)
                {
                    var order = new BGVOrder();

                    if (bgvTag.Attribute(namesAttribute) != null)
                    {
                        order.FileNames = new List<string>(bgvTag.Attribute(namesAttribute).Value.Split(','));
                        order.FileNames = order.FileNames.Select(name => name.Trim()).ToList();
                    }

                    if (bgvTag.Attribute(channelAttribute) != null)
                    {
                        order.Channel = int.Parse(bgvTag.Attribute(channelAttribute).Value);
                    }

                    scenario.BGVOrders.Add(order);
                }
            }
        }
    }
}
