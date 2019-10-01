using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemGetTooltip : MonoBehaviour //gets and sends information to the tool tip so that it can be displayed to the player
{
    private MouseToolTip[] mouseToolTips;
    private MouseToolTip toolTip;
    private ItemBuy iBuy;
    private Item thisItem;
    List<Item> childItems = new List<Item>();

    private void Start()
    {
        iBuy = GetComponent<ItemBuy>();
        mouseToolTips = Resources.FindObjectsOfTypeAll<MouseToolTip>();
        toolTip = mouseToolTips[0];

        foreach (Item childItem in transform.parent.parent.GetComponentsInChildren<Item>())
        {
            childItems.Add(childItem);
        }

        for (int i = 0; i < childItems.Count; i++)
        {
            if (childItems[i].itemStats.displayName == iBuy.itemName)
            {
                thisItem = childItems[i];
                thisItem.gameObject.SetActive(false);
                break;
            }
        }
    }

    private void OnMouseEnter()
    {
        toolTip.ShowShopItemInfo(thisItem);
    }

    private void OnMouseExit()
    {
        toolTip.HideToolTip();
    }
}
