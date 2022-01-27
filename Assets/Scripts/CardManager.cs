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
    [SerializeField] private Vector3 hoveredCardScale = Vector3.one;
    [Space]
    [SerializeField] private Vector3 selectedCardOffset = Vector3.zero;
    [SerializeField] private Vector3 selectedCardScale = Vector3.one;
    [Space]
    [SerializeField] private GameObject sideAPlacementArea;
    [SerializeField] private GameObject sideBPlacementArea;

    private GameObject selectedCard = null;

    //void Start()
    //{    
    //}

    int GetHoveredIndex()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<Card>().Hovered)
                return i;
        }
        return -1;
    }

    void Update()
    {
        float lerp = lerpFactor * Time.deltaTime;

        //Unselected Cards
        int hoveredIndex = GetHoveredIndex();
        for(int i = 0; i < transform.childCount; i++)
        {
            int offsetIndex = hoveredIndex == -1 ? Mathf.Abs(i - ((transform.childCount - 1) / 2)) : Mathf.Abs(i - hoveredIndex);

            Transform tr = transform.GetChild(i);
            SpriteRenderer spRend = tr.GetComponent<SpriteRenderer>();
            spRend.sortingOrder = 1;
            spRend.color = Color.Lerp(spRend.color, Color.Lerp(startingColor, endColor, colorLerpPerCard * offsetIndex), lerp);

            Vector3 position = tr.position;
            position.x = startingPosition.x - (xOffsetPerCard * transform.childCount / 2) + (xOffsetPerCard * i);
            position.y = startingPosition.y - ((hoveredIndex == -1 ? yOffsetPerCard : hoveredYOffset) * offsetIndex);

            Vector3 rotation = tr.rotation.eulerAngles;
            rotation.z = startingAngle + (angleOffsetPerCard * i);

            Vector3 scale = startingScale;

            if(tr.GetComponent<Card>().Hovered)
            {
                position.y += hoveredYOffset;
                scale = hoveredCardScale;

                spRend.sortingOrder = 10;

                if(Input.GetMouseButtonDown(0) && selectedCard == null)
                {
                    selectedCard = tr.gameObject;
                    selectedCard.transform.parent = null;

                    selectedCard.transform.GetChild(0).gameObject.SetActive(true);

                    sideAPlacementArea.SetActive(true);

                    spRend.sortingOrder = 10;
                    spRend.color = startingColor;
                }
            }

            tr.position = Vector3.Lerp(tr.position, position, lerp);
            tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.Euler(rotation), lerp);
            tr.localScale = Vector3.Lerp(tr.localScale, scale, lerp);
        }

        //Selected Card
        if(selectedCard != null)
        {
            if(Input.GetMouseButton(0))
            {
                selectedCard.transform.localScale = Vector3.Lerp(selectedCard.transform.localScale, selectedCardScale, lerp);
                selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, Utility.MousePos() + selectedCardOffset, lerp);
                selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.identity, lerp);
            }
            else
            {
                selectedCard.GetComponent<Card>().Action(Utility.MousePos());
                ImageEffectController.instance.invert = !ImageEffectController.instance.invert;

                selectedCard.transform.GetChild(0).gameObject.SetActive(false);

                selectedCard.transform.parent = transform;
                selectedCard = null;

                sideAPlacementArea.SetActive(false);
            }
        }
    }
}
