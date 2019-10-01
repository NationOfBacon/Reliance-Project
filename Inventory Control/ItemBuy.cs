using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBuy : MonoBehaviour
{
    private Inventory loadoutInv;
    private Inventory mainInv;
    private InventoryManager invManager;
    private MultipleAudioClips mClips;
    private string itemPrice;
    public string itemName;
    public GameObject attachedItem;
    private GameObject shopPanel;
    private Item sentItem;

    private void Awake()
    {
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        mainInv = invManager.inv;
        loadoutInv = invManager.inv2;
        shopPanel = transform.parent.gameObject;

        mClips = GetComponent<MultipleAudioClips>();
    }

    private void Start()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "Item Cost")
            {
                itemPrice = child.gameObject.GetComponent<TextMeshProUGUI>().text;
            }

            if (child.gameObject.name == "Item Name Text")
            {
                itemName = child.gameObject.GetComponent<TextMeshProUGUI>().text;
            }
        }
    }

    public void BuyItem()
    {
        if(itemName.Contains("Altered")) //use the datum currency if the item is "Altered"
        {
            //check if the player has enough money
            if (mainInv.datumCurrency >= int.Parse(itemPrice))
            {
                //send item to inventory
                loadoutInv.ItemRetrieve(attachedItem.GetComponent<Item>().itemStats);
                mainInv.ItemRetrieve(attachedItem.GetComponent<Item>().itemStats);

                //reduce currency
                mainInv.datumCurrency -= int.Parse(itemPrice);
                invManager.currency1 -= int.Parse(itemPrice);

                //remove item from shop and add to player inventory at Inventory Manager
                invManager.allPickedUpObjects.Add(attachedItem.GetComponent<Item>().itemStats.thisObject);
                invManager.OnItemMove(attachedItem.GetComponent<Item>().gameObject, true);
                attachedItem.GetComponent<Item>().itemStats.thisObject.transform.parent = invManager.transform;
                Destroy(this.gameObject);
            }
            else
                OnFailToBuy();
        }
        else
        {
            //check if the player has enough money
            if (mainInv.creditCurrency >= int.Parse(itemPrice))
            {
                //send item to inventory
                loadoutInv.ItemRetrieve(attachedItem.GetComponent<Item>().itemStats);
                mainInv.ItemRetrieve(attachedItem.GetComponent<Item>().itemStats);

                //reduce currency
                mainInv.creditCurrency -= int.Parse(itemPrice);
                invManager.currency -= int.Parse(itemPrice);

                //remove item from shop and add to player inventory at Inventory Manager
                invManager.allPickedUpObjects.Add(attachedItem.GetComponent<Item>().itemStats.thisObject);
                invManager.OnItemMove(attachedItem.GetComponent<Item>().gameObject, true);
                attachedItem.GetComponent<Item>().itemStats.thisObject.transform.parent = invManager.transform;
                Destroy(this.gameObject);
            }
            else
                OnFailToBuy();
        }
    }

    public void OnFailToBuy()
    {
        mClips.PlayClip(0, 1f);
    }
}
