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
    [Space]
    [SerializeField] private TextMeshPro goodText;
    [SerializeField] private TextMeshPro badText;

    private int propertyIndex = -1;

    public int MPCost { get { return properties[propertyIndex].mpCost; } }

    void Start()
    {
        propertyIndex = Range(0, properties.Length);

        characterRenderer.sprite = properties[propertyIndex].character;
        title.text = properties[propertyIndex].name;
        mpCost.text = properties[propertyIndex].mpCost.ToString();
    }

    public void Update()
    {
        goodText.gameObject.SetActive(!ImageEffectController.instance.invert);
        badText.transform.parent.gameObject.SetActive(ImageEffectController.instance.invert);
    }

    public void Action(Vector3 position, MP mp, GameObject parentObj)
    {
        mp.Value -= properties[propertyIndex].mpCost;

        GameObject newObj = Instantiate(objectToSpawn, position, Quaternion.identity);
        newObj.transform.parent = parentObj.transform;
        Unit unit = newObj.GetComponent<Unit>();
        if(unit != null)
        {
            newObj.tag = "A";
            unit.SetTargetUsingTag("BaseB");
            unit.SetAvoidTag("BaseA");
            unit.SetProperties(properties[propertyIndex]);
        }
    }
}
