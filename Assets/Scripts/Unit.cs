using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private string targetTag = "";
    [SerializeField] private string avoidTag = "";
    [SerializeField] private SpriteRenderer characterRenderer = null;
    [SerializeField] private Animator characterAnimator = null;
    [SerializeField] private UnitProperties properties = null;

    private Rigidbody2D rb;
    private GameObject target;

    private bool stop = false;
    private bool attack = false;

    private Health healthToAttack = null;

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

    public SpriteRenderer GetCharacterRenderer()
    {
        return characterRenderer;
    }

    public void SetTargetUsingTag(string tag)
    {
        targetTag = tag;
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    public void SetAvoidTag(string tag)
    {
        avoidTag = tag;
    }

    public void Attack()
    {
        attack = true;
    }

    void Update()
    {
        if(target != null && !stop)
        {
            characterAnimator.SetBool("Walk", true);
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, properties.speed * Time.deltaTime));
        }
        else
        {
            rb.velocity = Vector3.zero;
            characterAnimator.SetBool("Walk", false);
        }

        if(attack)
        {
            if(healthToAttack != null)
                healthToAttack.Change(-properties.damage);
            attack = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == tag || coll.gameObject.tag == avoidTag)
        {
            if(!stop) rb.MovePosition(transform.position + transform.up / 100.0f);
            return;
        }
        
        stop = true;
        characterAnimator.SetBool("Attack", true);

        Health health = coll.gameObject.GetComponent<Health>();
        if(health != null) healthToAttack = health;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if(coll.gameObject.tag == tag || coll.gameObject.tag == avoidTag) return;
        
        stop = false;
        characterAnimator.SetBool("Attack", false);
    }
}
