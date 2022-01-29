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
    [Space]
    [SerializeField] private float xMin = 0.0f;
    [SerializeField] private float xMax = 0.0f;
    [SerializeField] private float yMin = 0.0f;
    [SerializeField] private float yMax = 0.0f;

    [HideInInspector] public Vector3 intention = Vector3.zero;
    [HideInInspector] public float speedMultiplier = 1.0f;

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

    public GameObject GetTarget()
    {
        return target;
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

            if(intention == Vector3.zero)
                rb.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, (properties.speed * speedMultiplier) * Time.deltaTime));
            else
                rb.MovePosition(transform.position + (intention * (properties.speed * speedMultiplier) * Time.deltaTime));
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

        Vector3 pos = transform.position;
        if(pos.x < xMin) pos.x = xMin;
        else if(pos.x > xMax) pos.x = xMax;
        if(pos.y < yMin) pos.y = yMin;
        else if(pos.y > yMax) pos.y = yMax;
        transform.position = pos;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(xMin, yMin, 0.0f), new Vector3(xMax, yMin, 0.0f));
        Gizmos.DrawLine(new Vector3(xMin, yMax, 0.0f), new Vector3(xMax, yMax, 0.0f));
        Gizmos.DrawLine(new Vector3(xMin, yMin, 0.0f), new Vector3(xMin, yMax, 0.0f));
        Gizmos.DrawLine(new Vector3(xMax, yMin, 0.0f), new Vector3(xMax, yMax, 0.0f));
    }
}
