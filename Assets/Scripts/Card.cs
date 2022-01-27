using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private bool hovered = false;

    public bool Hovered { get { return hovered; } }

    public void Action(Vector3 position)
    {
        Instantiate(objectToSpawn, position, Quaternion.identity);
    }

    void OnMouseOver()
    {
        hovered = true;
    }

    void OnMouseExit()
    {
        hovered = false;
    }
}
