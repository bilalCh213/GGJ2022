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
        obj.tag = objectTag;
        Unit unit = obj.GetComponent<Unit>();
        if(unit != null)
        {
            unit.SetTargetUsingTag(targetTag);
            unit.SetProperties(properties[Range(0, properties.Length)]);
            unit.GetCharacterRenderer().flipX = flipX;
        }
    }
}
