using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class TextLoader
{
    public void Load(string targetPath)
    {
        try
        {
            XDocument xml = XDocument.Parse(File.ReadAllText(targetPath));

            foreach (var data in xml.Root.Descendants())
            {
                Debug.Log(data);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
