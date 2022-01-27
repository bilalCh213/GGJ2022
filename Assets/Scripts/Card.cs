using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool hovered = false;

    public bool Hovered { get { return hovered; } }

    //void Start()
    //{
    //}

    //void Update()
    //{
    //}

    void OnMouseOver()
    {
        hovered = true;
    }

    void OnMouseExit()
    {
        hovered = false;
    }
}
