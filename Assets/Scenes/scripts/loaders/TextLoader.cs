using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class TextLoader
{
    public List<Scenario> Scenario { get; set; }

    public void Load(string targetPath)
    {
        try
        {
            XDocument xml = XDocument.Parse(File.ReadAllText(targetPath));

            Scenario =
            xml.Root.Descendants().Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario").Select(x =>
            {
                var scenario = new Scenario() { Text = x.Element("text").Attribute("str").Value };
                return scenario;
            }).ToList();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
