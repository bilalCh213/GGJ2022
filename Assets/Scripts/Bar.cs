using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bar : MonoBehaviour
{
    [Range(0, 1)] public float value = 0.0f;
    [Space]
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public Vector3 minScale;
    public Vector3 maxScale;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(minPosition, maxPosition, value);
        transform.localScale = Vector3.Lerp(minScale, maxScale, value);
    }
}
