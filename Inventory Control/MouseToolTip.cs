using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseToolTip : MonoBehaviour
{
    public TextMeshProUGUI toolText;
    public RectTransform bgRect;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localPoint);
        transform.localPosition = localPoint;
    }

    public void ShowSlotInfo(Slot targetSlot) //used to display information about an item in the inventory
    {
        gameObject.SetActive(true);

        if (targetSlot.item != null)
        {
            toolText.text = targetSlot.item.GetComponent<Item>().itemStats.displayName; //show the items name

            toolText.text += "\n" + targetSlot.description + "\n"; //show the item description and then leave a space

            for (int i = 0; i < targetSlot.bonusStats.Count; i++)
            {
                if(targetSlot.bonusStats[i] != 0) //show all bonus stats the item has
                {
                    toolText.text += "\n" + targetSlot.bonusNames[i] + ": " + targetSlot.bonusStats[i];
                }
            }
            if (targetSlot.item.GetComponent<Item>().itemStats.displayName.Contains("Altered"))
                toolText.text += "\n\nSell value: " + targetSlot.item.GetComponent<Item>().itemStats.sellPrice + " datum";
            else
                toolText.text += "\n\nSell value: " + targetSlot.item.GetComponent<Item>().itemStats.sellPrice + " credits";
        }
        else
            toolText.text = "empty";
    }

    public void HideSlotInfo() //hides the tool tip
    {
        gameObject.SetActive(false);
    }

    public void ShowToolTip(string toolString) //used to show a string of info on the tooltip
    {
        gameObject.SetActive(true);

        toolText.text = toolString;
    }

    public void ShowShopItemInfo(Item targetItem) //used to show info for items that are sold from shops
    {
        gameObject.SetActive(true);

        if (targetItem != null)
        {
            toolText.text = targetItem.GetComponent<Item>().itemStats.displayName; //show the items name

            toolText.text += "\n" + targetItem.itemStats.description + "\n"; //show the item description and then leave a space

            for (int i = 0; i < targetItem.bonusStats.Count; i++)
            {
                if (targetItem.bonusStats[i] != 0) //show all bonus stats the item has
                {
                    toolText.text += "\n" + targetItem.bonusNames[i] + ": " + targetItem.bonusStats[i];
                }
            }
        }
        else
            toolText.text = "empty";
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public void ShowPerkInfo(Perk targetPerk) //used to show info about a perk on the tooltip
    {
        var statStruct = targetPerk.stats;

        gameObject.SetActive(true);
        //get the perk info and display it
        toolText.text = statStruct.perkName + "\n" + statStruct.perkDesc;
    }
}
