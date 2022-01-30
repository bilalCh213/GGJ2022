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
    [Space]
    [SerializeField] private string[] goodEffects;
    [SerializeField] private string[] badEffects;

    private int propertyIndex = -1;
    private int effectIndex = -1;

    [HideInInspector] public bool otherText = false;

    public int MPCost { get { return properties[propertyIndex].mpCost; } }

    void Start()
    {
        propertyIndex = Range(0, properties.Length);
        characterRenderer.sprite = properties[propertyIndex].character;
        title.text = properties[propertyIndex].name;
        mpCost.text = properties[propertyIndex].mpCost.ToString();

        effectIndex = Range(0, (goodEffects.Length + badEffects.Length) / 2);
        goodText.text = goodEffects[effectIndex];
        badText.text = badEffects[effectIndex];
    }

    void Update()
    {
        bool bad = ImageEffectController.instance.invert || (otherText && !ImageEffectController.instance.invert);
        goodText.gameObject.SetActive(!bad);
        badText.transform.parent.gameObject.SetActive(bad);
    }

    public void Action(Vector3 position, MP mp, GameObject parentObj)
    {
        if(!ImageEffectController.instance.invert)
        {
            switch(effectIndex)
            {
                case 0: //An additional unit gets spawned!
                Spawn(position, parentObj);
                break;
                case 1: //No MP will be depleted!
                mp.Value += properties[propertyIndex].mpCost;
                break;
                case 2: //MP regen. gets boosted temporarily!
                mp.regenBoost += 8.0f;
                break;
                case 3: //2 cards will be added to your hand.
                CardManager.instance.AddCard();
                CardManager.instance.AddCard();
                break;
                case 4: //All of your current units gets faster.
                GameManager.instance.IncreaseUnitsSpeed();
                break;
                case 5: //All of your current units will replenish HP.
                GameManager.instance.ResetUnitsHealth();
                break;
                case 6: //Your base HP gets replenished.
                GameManager.instance.ResetBaseHealth();
                break;
                case 7: //All enemy units gets destroyed!
                GameManager.instance.DestroyAllUnits(false);
                break;
            }
        }
        else
        {
            switch(effectIndex)
            {
                case 0: //One of your unit gets destroyed!
                GameManager.instance.DestroyRandomUnit();
                break;
                case 1: //Double MP gets depleted!
                mp.Value -= properties[propertyIndex].mpCost;
                break;
                case 2: //Temporary MP degeneration!
                mp.regenBoost -= 8.0f;
                break;
                case 3: //2 cards will be removed from your hand.
                CardManager.instance.RemoveRandomCard();
                CardManager.instance.RemoveRandomCard();
                break;
                case 4: //All of your current units gets slower.
                GameManager.instance.DecreaseUnitsSpeed();
                break;
                case 5: //All of your current units' HP gets half.
                GameManager.instance.HalfUnitsHealth();
                break;
                case 6: //Your base HP gets halved.
                GameManager.instance.HalfBaseHealth();
                break;
                case 7: //All your units gets destroyed!
                GameManager.instance.DestroyAllUnits();
                break;
            }
        }

        mp.Value -= properties[propertyIndex].mpCost;
        Spawn(position, parentObj);
    }

    void Spawn(Vector3 position, GameObject parentObj)
    {
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
