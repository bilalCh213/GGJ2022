using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float max = 1.0f;
    [SerializeField] private Bar bar;
    [Space]
    [SerializeField] private bool destroyOnZero = true;
    [Space]
    [SerializeField] private AudioClip unitADestroyClip;
    [SerializeField] private AudioClip unitBDestroyClip;
    [SerializeField] private GameObject destroyParticles;

    private float current = 1.0f;

    public void Change(float change)
    {
        current += change;

        if(current > max) current = max;
        else if(current < 0) current = 0;

        bar.value = current/max;

        if(destroyOnZero && current <= 0)
        {
            if(tag == "A") { Instantiate(destroyParticles, transform.position, Quaternion.identity); Camera.main.GetComponent<AudioSource>().PlayOneShot(unitADestroyClip); }
            if(tag == "B") { Instantiate(destroyParticles, transform.position, Quaternion.identity); Camera.main.GetComponent<AudioSource>().PlayOneShot(unitBDestroyClip); }
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

    public void Half()
    {
        current /= 2.0f;
        bar.value = current/max;
    }

    void Start()
    {
        Reset();
    }
}
