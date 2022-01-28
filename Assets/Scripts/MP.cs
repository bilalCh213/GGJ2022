using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MP : MonoBehaviour
{
    [SerializeField] private float max = 100.0f;
    [SerializeField] private float regenRate = 2.0f;
    [SerializeField] private SpriteMask radialMask;
    [SerializeField] private TextMeshPro valueText;

    private float value = 0;

    public float Value { get { return value; } set { this.value = value; } }

    void Start()
    {
        value = max;
        Update();
    }

    void Update()
    {
        value += regenRate * Time.deltaTime;
        if(value > max) value = max;
        radialMask.alphaCutoff = value/max;
        valueText.text = Mathf.FloorToInt(value).ToString() + "/" + Mathf.FloorToInt(max).ToString();
    }
}
