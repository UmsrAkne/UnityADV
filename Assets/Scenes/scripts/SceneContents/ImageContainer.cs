﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageContainer
{
    private GameObject gameObject;

    public GameObject GameObject
    {
        get => gameObject;
        set
        {
            if (gameObject == null)
            {
                gameObject = value;
            }
        }
    }

    private List<GameObject> Childs { get; } = new List<GameObject>();

    public GameObject FrontChild => Childs.FirstOrDefault();

    public void AddChild(GameObject childObject)
    {
        childObject.transform.SetParent(GameObject.transform);
        Childs.Add(childObject);
    }
}
