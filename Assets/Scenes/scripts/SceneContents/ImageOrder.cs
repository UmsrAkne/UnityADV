using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOrder
{
    public List<string> Names { get; private set; } = new List<string>();

    public int TargetLayerIndex { get; set; }

    public double Scale { get; set; } = 1.0;
}
