using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private UnitProperties[] properties;
    [Space]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private TextMeshPro title;

    private int propertyIndex = -1;

    private bool hovered = false;

    public bool Hovered { get { return hovered; } }

    void Start()
    {
        propertyIndex = Range(0, properties.Length);

        title.text = properties[propertyIndex].name;
        characterRenderer.sprite = properties[propertyIndex].character;
    }

    public void Action(Vector3 position)
    {
        GameObject newObj = Instantiate(objectToSpawn, position, Quaternion.identity);
        Unit unit = newObj.GetComponent<Unit>();
        if(unit != null)
        {
            newObj.tag = "A";
            unit.SetTargetUsingTag("BaseB");
            unit.SetProperties(properties[propertyIndex]);
        }
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
