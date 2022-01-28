using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private string targetTag = "";
    [SerializeField] private SpriteRenderer characterRenderer = null;
    [SerializeField] private UnitProperties properties = null;

    private Rigidbody2D rb;
    private GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag(targetTag);
        SetProperties(properties);
    }

    public void SetProperties(UnitProperties properties)
    {
        this.properties = properties;
        GetComponent<Health>().Set(properties.health);
        characterRenderer.sprite = properties.character;
    }

    public void SetTargetUsingTag(string tag)
    {
        targetTag = tag;
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    void Update()
    {
        if(target != null)
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, properties.speed * Time.deltaTime));
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == tag) return;
        Health health = coll.gameObject.GetComponent<Health>();
        if(health != null)
            health.Change(-properties.damage * Time.deltaTime); 
    }
}
