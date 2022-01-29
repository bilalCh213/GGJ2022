using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCheck : MonoBehaviour
{
    private bool hovered = false;
    public bool Hovered { get { return hovered; } }
    void OnDisable() { hovered = false; }
    void OnMouseOver() { hovered = true; }
    void OnMouseExit() { hovered = false; }
}
