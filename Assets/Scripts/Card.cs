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
    [SerializeField] private TextMeshPro mpCost;

    private int propertyIndex = -1;

    private bool hovered = false;

    public bool Hovered { get { return hovered; } }
    public int MPCost { get { return properties[propertyIndex].mpCost; } }

    void Start()
    {
        propertyIndex = Range(0, properties.Length);

        characterRenderer.sprite = properties[propertyIndex].character;
        title.text = properties[propertyIndex].name;
        mpCost.text = properties[propertyIndex].mpCost.ToString();
    }

    public void Action(Vector3 position, MP mp)
    {
        mp.Value -= properties[propertyIndex].mpCost;

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
