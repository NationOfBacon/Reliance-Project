using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool attached;
    private Transform startingParent;
    private Inventory inv;

    private void Awake()
    {
        attached = false;
        startingParent = transform.parent;
        inv = GameObject.Find("HUBUI/Loadout Panel/Inventory Grid").GetComponent<Inventory>();
    }

    public void OnPointerDown(PointerEventData eventData) //when you click on a slot, move the slot image above all other UI items and ensure that it cannot block raycasts.
    {
        if (inv.slot[transform.parent.GetSiblingIndex()] != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                attached = true;
                startingParent = transform.parent;
                transform.SetParent(transform.parent.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData) //while dragging the item, set its position to the mouse position
    {
        if (inv.slot[transform.parent.GetSiblingIndex()] != null && eventData.button == PointerEventData.InputButton.Left)
        {

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localPoint);
            transform.localPosition = localPoint;

            //transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData) //when the mouse is released, reset the slots position back to where you dragged it from
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            attached = false;
            transform.SetParent(startingParent);
            transform.localPosition = Vector3.zero;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
