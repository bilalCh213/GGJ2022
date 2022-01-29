using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardObject;
    [Space]
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
    [SerializeField] private float unselectableYOffset = 0.0f;
    [Space]
    [SerializeField] private Vector3 selectedCardOffset = Vector3.zero;
    [SerializeField] private Vector3 selectedCardScale = Vector3.one;
    [Space]
    [SerializeField] private MP mp;
    [SerializeField] private int addCardMPCost = 10;
    [Space]
    [SerializeField] private GameObject unitsA;
    [SerializeField] private GameObject sideAPlacementArea;
    [SerializeField] private GameObject sideBPlacementArea;
    [SerializeField] private Vector2 minPlacement = Vector2.zero;
    [SerializeField] private Vector2 maxPlacement = Vector2.zero;
    [Space]
    [SerializeField] private GameObject addCardArea;
    [SerializeField] private GameObject removeCardArea;

    private GameObject selectedCard = null;

    int GetHoveredIndex()
    {
        for(int i = 0; i < transform.childCount; i++)
            if(transform.GetChild(i).GetComponent<HoverCheck>().Hovered)
                return i;
        return -1;
    }

    bool IsInsidePlacement(Vector2 pos)
    {
        return pos.x >= minPlacement.x && pos.x < maxPlacement.x && pos.y >= minPlacement.y && pos.y < maxPlacement.y;
    }

    void Update()
    {
        float lerp = lerpFactor * Time.deltaTime;

        //Displaying Add Card Option when cards are quite less
        addCardArea.SetActive(transform.childCount <= 3);

        //Processing Unselected Cards
        int hoveredIndex = GetHoveredIndex();
        for(int i = 0; i < transform.childCount; i++)
        {
            int offsetIndex = hoveredIndex == -1 ? Mathf.Abs(i - ((transform.childCount - 1) / 2)) : Mathf.Abs(i - hoveredIndex);

            Transform tr = transform.GetChild(i);
            SpriteRenderer blackOverlayRend = tr.GetChild(1).GetComponent<SpriteRenderer>();

            bool selectable = tr.gameObject.GetComponent<Card>().MPCost <= mp.Value;

            //Change in color helps in distinguishing cards
            blackOverlayRend.color = Color.Lerp(blackOverlayRend.color, Color.Lerp(startingColor, endColor, colorLerpPerCard * offsetIndex), lerp);

            //Cards in the hand (bottom center group of cards) are placed in an cool looking and appropriate manner
            Vector3 position = tr.position;
            position.x = startingPosition.x - (xOffsetPerCard * transform.childCount / 2) + (xOffsetPerCard * i);
            position.y = startingPosition.y - ((hoveredIndex == -1 ? yOffsetPerCard : hoveredYOffset) * offsetIndex) + (selectable ? 0.0f : unselectableYOffset);

            //A little rotation between cards look cool!
            Vector3 rotation = tr.rotation.eulerAngles;
            rotation.z = startingAngle + (angleOffsetPerCard * i);

            Vector3 scale = startingScale;

            //When card is hovered...
            if(tr.GetComponent<HoverCheck>().Hovered)
            {
                if(selectable)
                {
                    position.y += hoveredYOffset;
                    position.z = -1.0f;
                    rotation.y = 0.0f;
                    scale = hoveredCardScale;

                    //When hovered card is selected...
                    if(Input.GetMouseButtonDown(0) && selectedCard == null)
                    {
                        selectedCard = tr.gameObject;

                        //Card gets out of the Card Manager's transform so that, it is no longer included in the hand (bottom center group of cards)
                        selectedCard.transform.parent = null;

                        //Each card got an selection indicator that is activated upon selection
                        selectedCard.transform.GetChild(0).gameObject.SetActive(true);

                        //sideAPlacementArea is the player's placement area
                        sideAPlacementArea.SetActive(true);

                        //Remove Card Option is displayed when a card is selected
                        removeCardArea.SetActive(true);
                        removeCardArea.transform.localScale = Vector3.one * 0.8f;

                        blackOverlayRend.color = startingColor;
                    }
                }
                else if(Input.GetMouseButtonDown(0))
                {
                    tr.localScale = Vector3.one * 0.4f;
                }
            }
            else
            {
                position.z = 0.0f;
            }

            //All three transform properties for all cards
            tr.position = Vector3.Lerp(tr.position, position, lerp);
            tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.Euler(rotation), lerp);
            tr.localScale = Vector3.Lerp(tr.localScale, scale, lerp);
        }

        //Processing Selected Card
        if(selectedCard != null)
        {
            bool removeHover = removeCardArea.GetComponent<HoverCheck>().Hovered;

            //When dragging the selected card...
            if(Input.GetMouseButton(0))
            {
                //...Keep updating the position to that of mouse position
                selectedCard.transform.position = Vector3.Lerp(selectedCard.transform.position, Utility.MousePos() + selectedCardOffset, lerp);
                selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.identity, lerp);
                selectedCard.transform.localScale = Vector3.Lerp(selectedCard.transform.localScale, selectedCardScale, lerp);

                removeCardArea.transform.localScale = Vector3.Lerp(removeCardArea.transform.localScale, Vector3.one * (removeHover ? 1.0f : 0.8f), lerpFactor * Time.deltaTime);
            }
            //When the selected card is no longer being dragged...
            else
            {
                if(removeHover)
                {
                    mp.Value -= selectedCard.GetComponent<Card>().MPCost / 2.0f;
                    Destroy(selectedCard);
                    selectedCard = null;
                }
                else
                {
                    Vector2 pos = Utility.MousePos();
                    if(IsInsidePlacement(pos))
                    {
                        //take action at the mouse position and invert the screen colors
                        selectedCard.GetComponent<Card>().Action(pos, mp, unitsA);
                        ImageEffectController.instance.invert = !ImageEffectController.instance.invert;
                        Destroy(selectedCard);
                    }
                    else
                    {
                        //Card's selection indicator is disabled
                        selectedCard.transform.GetChild(0).gameObject.SetActive(false);

                        //Card is back to the hand (bottom center group of cards)
                        selectedCard.transform.parent = transform;
                    }

                    selectedCard = null;

                    //Placement area is disabled
                    sideAPlacementArea.SetActive(false);
                }
            }
        }
        else
        {
            removeCardArea.SetActive(false);
        }

        //Add a card when clicking on Add Card Area
        if(addCardArea.GetComponent<HoverCheck>().Hovered
        && Input.GetMouseButtonDown(0)
        && mp.Value >= addCardMPCost)
        {
            mp.Value -= addCardMPCost;
            GameObject newCardObject = Instantiate(cardObject, addCardArea.transform.position, Quaternion.identity);
            newCardObject.transform.parent = transform;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(minPlacement.x, minPlacement.y), new Vector3(minPlacement.x, maxPlacement.y));
        Gizmos.DrawLine(new Vector3(minPlacement.x, minPlacement.y), new Vector3(maxPlacement.x, minPlacement.y));
        Gizmos.DrawLine(new Vector3(minPlacement.x, maxPlacement.y), new Vector3(maxPlacement.x, maxPlacement.y));
        Gizmos.DrawLine(new Vector3(maxPlacement.x, minPlacement.y), new Vector3(maxPlacement.x, maxPlacement.y));
    }
}
