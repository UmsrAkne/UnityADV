using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOrder
{
    public List<string> Names { get; private set; } = new List<string>();

    public int TargetLayerIndex { get; set; }

    public double Scale { get; set; } = 1.0;

    public int X { get; set; }

    public int Y { get; set; }

    public int Angle { get; set; }

    public bool IsDrawOrder { get; set; }
}
