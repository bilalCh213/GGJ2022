using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] spawnObjects;
    [SerializeField] protected float minDelay = 4.0f;
    [SerializeField] protected float maxDelay = 8.0f;
    [Space]
    [SerializeField] protected Vector2 minSpawnPosition = Vector3.zero;
    [SerializeField] protected Vector2 maxSpawnPosition = Vector3.zero;

    protected List<GameObject> objects = new List<GameObject>();
    protected float timer = 0.0f;

    void Start()
    {
        timer = Range(minDelay, maxDelay);
    }

    virtual protected GameObject GetRandomSpawnedObject()
    {
        return Instantiate(spawnObjects[Range(0, spawnObjects.Length)], transform.position + new Vector3(Range(minSpawnPosition.x, maxSpawnPosition.x), Range(minSpawnPosition.y, maxSpawnPosition.y), 0.0f), Quaternion.identity);
    }

    virtual protected void ModifySpawnedObject(GameObject obj)
    {
    }

    void Update()
    {
        if(timer <= 0.0f)
        {
            GameObject newObj = GetRandomSpawnedObject();
            ModifySpawnedObject(newObj);
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
        Gizmos.DrawLine(new Vector2(transform.position.x + minSpawnPosition.x, transform.position.y + minSpawnPosition.y), new Vector2(transform.position.x + maxSpawnPosition.x, transform.position.y + minSpawnPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + minSpawnPosition.x, transform.position.y + minSpawnPosition.y), new Vector2(transform.position.x + minSpawnPosition.x, transform.position.y + maxSpawnPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + maxSpawnPosition.x, transform.position.y + maxSpawnPosition.y), new Vector2(transform.position.x + minSpawnPosition.x, transform.position.y + maxSpawnPosition.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + maxSpawnPosition.x, transform.position.y + maxSpawnPosition.y), new Vector2(transform.position.x + maxSpawnPosition.x, transform.position.y + minSpawnPosition.y));
    }
}
