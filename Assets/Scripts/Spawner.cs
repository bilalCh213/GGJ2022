using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private float minDelay = 4.0f;
    [SerializeField] private float maxDelay = 8.0f;
    [Space]
    [SerializeField] private Vector2 minPosition = Vector3.zero;
    [SerializeField] private Vector2 maxPosition = Vector3.zero;

    private List<GameObject> objects = new List<GameObject>();
    private float timer = 0.0f;

    void Start()
    {
        timer = Range(minDelay, maxDelay);
    }

    void Update()
    {
        if(timer <= 0.0f)
        {
            GameObject newObj = Instantiate(spawnObjects[Range(0, spawnObjects.Length)], transform.position + new Vector3(Range(minPosition.x, maxPosition.x), Range(minPosition.y, maxPosition.y), 0.0f), Quaternion.identity);
            newObj.transform.parent = transform;
            objects.Add(newObj);
            timer = Range(minDelay, maxDelay);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(transform.position.x + minPosition.x, transform.position.y + minPosition.y), new Vector2(transform.position.x + maxPosition.x, transform.position.y + minPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + minPosition.x, transform.position.y + minPosition.y), new Vector2(transform.position.x + minPosition.x, transform.position.y + maxPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + maxPosition.x, transform.position.y + maxPosition.y), new Vector2(transform.position.x + minPosition.x, transform.position.y + maxPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + maxPosition.x, transform.position.y + maxPosition.y), new Vector2(transform.position.x + maxPosition.x, transform.position.y + minPosition.y));
    }
}
