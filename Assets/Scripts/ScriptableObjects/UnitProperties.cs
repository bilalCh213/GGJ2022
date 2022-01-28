using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Unit", fileName = "New Unit")]
public class UnitProperties : ScriptableObject
{
    public Sprite character;
    public float health;
    public float damage;
    public float speed;
}
