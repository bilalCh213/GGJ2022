using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToAndFro : MonoBehaviour
{
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float distance = 10.0f;

    private float dist = 0.0f;
    private bool alt = false;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime * (alt ? -1.0f : 1.0f);
        dist += speed * Time.deltaTime;

        if(dist > distance)
        {
            dist = 0.0f;
            alt = true;
        }
    }
}
