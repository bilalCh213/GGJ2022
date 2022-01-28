using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class UnitSpawner : Spawner
{
    [Space]
    [SerializeField] private string objectTag = "";
    [SerializeField] private string targetTag = "";
    [SerializeField] private bool flipX = true;
    [SerializeField] private UnitProperties[] properties;

    override protected void ModifySpawnedObject(GameObject obj)
    {
        gameObject.tag = objectTag;
        obj.GetComponent<Unit>().SetTargetUsingTag(targetTag);
        obj.GetComponent<Unit>().SetProperties(properties[Range(0, properties.Length)]);
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = flipX;
    }
}
