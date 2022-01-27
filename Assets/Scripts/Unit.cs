using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private string targetTag = "";
    [SerializeField] private float speed = 1.0f;

    private Rigidbody2D rb;
    private GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    void Update()
    {
        if(target != null)
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime));
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        Health health = coll.gameObject.GetComponent<Health>();
        if(health != null)
            health.Change(-0.25f * Time.deltaTime); 
    }
}
