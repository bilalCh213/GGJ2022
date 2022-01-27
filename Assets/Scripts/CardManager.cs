using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private Color startingColor = Color.white;
    [SerializeField] private Vector3 startingPosition = Vector3.zero;
    [SerializeField] private float startingAngle = 0.0f;
    [SerializeField] private Vector3 startingScale = Vector3.one;
    [Space]
    [SerializeField] private Color endColor = Color.black;
    [SerializeField] private float colorLerpPerCard = 0.01f;
    [SerializeField] private float xOffsetPerCard = 1.0f;
    [SerializeField] private float yOffsetPerCard = 0.1f;
    [SerializeField] private float angleOffsetPerCard = 5.0f;
    [Space]
    [SerializeField] private float hoveredYOffset = 0.0f;
    [SerializeField] private Vector3 hoveredScale = Vector3.one;
    [Space]
    [SerializeField] private Vector3 selectedCardOffset = Vector3.zero;
    [SerializeField] private Vector3 selectedScale = Vector3.one;

    private GameObject selectedCard = null;

    //void Start()
    //{    
    //}

    void Update()
    {
        //Unselected Cards
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.Lerp(startingColor, endColor, colorLerpPerCard * i);

            Vector3 position = transform.GetChild(i).position;
            position.x = startingPosition.x - (xOffsetPerCard * transform.childCount / 2) + (xOffsetPerCard * i);
            position.y = startingPosition.y + (yOffsetPerCard * i);

            Vector3 rotation = transform.GetChild(i).rotation.eulerAngles;
            rotation.z = startingAngle + (angleOffsetPerCard * i);

            transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position, position, lerpFactor * Time.deltaTime);
            transform.GetChild(i).rotation = Quaternion.Lerp(transform.GetChild(i).rotation, Quaternion.Euler(rotation), lerpFactor * Time.deltaTime);

            if(transform.GetChild(i).GetComponent<Card>().Hovered)
            {
                position.y += hoveredYOffset;
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, hoveredScale, lerpFactor * Time.deltaTime);

                transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 10;

                if(Input.GetMouseButtonDown(0) && selectedCard == null)
                {
                    selectedCard = transform.GetChild(i).gameObject;
                    selectedCard.transform.parent = null;

                    selectedCard.transform.GetChild(0).gameObject.SetActive(true);

                    selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    selectedCard.GetComponent<SpriteRenderer>().color = startingColor;
                }
            }
            else
            {
                transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, startingScale, lerpFactor * Time.deltaTime);

                transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }

        //Selected Card
        if(selectedCard != null)
        {
            if(Input.GetMouseButton(0))
            {
                selectedCard.transform.localScale = Vector3.Lerp(selectedCard.transform.localScale, selectedScale, lerpFactor * Time.deltaTime);
                selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, Utility.MousePos() + selectedCardOffset, lerpFactor * Time.deltaTime);
                selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.identity, lerpFactor * Time.deltaTime);
            }
            else
            {
                selectedCard.transform.GetChild(0).gameObject.SetActive(false);

                selectedCard.transform.parent = transform;
                selectedCard = null;
            }
        }
    }
}
