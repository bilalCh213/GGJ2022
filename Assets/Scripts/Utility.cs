using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    static public Camera cam = null;

    static public Vector3 MousePos()
    {
        if(cam == null) cam = Camera.main;
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        return pos;
    }
}
