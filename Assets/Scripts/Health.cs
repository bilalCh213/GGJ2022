using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float max = 1.0f;
    [SerializeField] private Bar bar;
    [Space]
    [SerializeField] private bool destroyOnZero = true;

    private float current = 1.0f;

    public void Change(float change)
    {
        current += change;

        if(current > max) current = max;
        else if(current < 0) current = 0;

        bar.value = current/max;

        if(destroyOnZero && current <= 0)
        {
            if(gameObject != bar.gameObject) Destroy(bar.gameObject);
            Destroy(gameObject);
        }
    }

    public void Set(float value)
    {
        max = value;
        Reset();
    }

    public void Reset()
    {
        current = max;
        bar.value = current/max;
    }

    void Start()
    {
        Reset();
    }
}
