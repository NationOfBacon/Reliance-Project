using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject item;
    private Slot droppedItem;
    private Slot dupeDroppedItem;
    public Slot tempTarget;
    public Slot dupeTempTarget;
    public int ID;
    public string type;
    public string description;
    public bool empty;
    public Sprite icon;
    public List<int> bonusStats = new List<int>();
    public List<string> bonusNames = new List<string>() { "STR", "ELEC", "AGI", "CBT", "SCN", "EFF", "HIT", "RET", "VENT", "OBJM", "REGEN", "LUCK", "TEAMP", "INFO" };
    public int strBonus;
    public int elecBonus;
    public int agiBonus;
    public int combatBonus;
    public int scanBonus;
    public int crWBonus;
    public int rWBonus;
    public int retBonus;
    public int ventBonus;
    public int objMBonus;
    public int regenBonus;
    public int luckBonus;
    public int teamPBonus;
    public int infoBonus;

    private InventoryManager invManager;
    private Inventory inv;
    private Inventory invEquip;
    private Inventory mainInventory;
    private HUBStats hubStats;

    public Image childIcon;

    private void Awake()
    {
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        inv = GameObject.Find("HUBUI/Loadout Panel/Inventory Grid").GetComponent<Inventory>();
        invEquip = GameObject.Find("HUBUI/Loadout Panel/Equipment Grid").GetComponent<Inventory>();
        mainInventory = GameObject.Find("HUBUI/Inventory Panel/Grid Layout").GetComponent<Inventory>();
        hubStats = GameObject.Find("HUBUI/Loadout Panel/Stats Backing").GetComponent<HUBStats>();
        childIcon = transform.GetChild(0).GetComponent<Image>();
        bonusStats.Add(strBonus);
        bonusStats.Add(elecBonus);
        bonusStats.Add(agiBonus);
        bonusStats.Add(combatBonus);
        bonusStats.Add(scanBonus);
        bonusStats.Add(crWBonus);
        bonusStats.Add(rWBonus);
        bonusStats.Add(retBonus);
        bonusStats.Add(ventBonus);
        bonusStats.Add(objMBonus);
        bonusStats.Add(regenBonus);
        bonusStats.Add(luckBonus);
        bonusStats.Add(teamPBonus);
        bonusStats.Add(infoBonus);
    }

    public void UpdateSlot()
    {
        if(item != null)
        {
            if (icon != null)
            {
                childIcon.sprite = icon;
                childIcon.enabled = true;
            }
            else
            {
                childIcon.sprite = null;
                childIcon.enabled = false;
            }
        }
        else
        {
            childIcon.sprite = null;
            childIcon.enabled = false;
        }

    }

    public void OnDrop(PointerEventData eventData) // when you drop a slot over another slot, make it change to that slot
    {
        // Use this to return the sibling index of the GameObject. If a GameObject shares a parent with other GameObjects and are on the same level 
        //(i.e. they share the same direct parent), these GameObjects are known as siblings. The sibling index shows where each GameObject sits in this sibling hierarchy.
        // droppedItem is being set to the index value game object found by GetSiblingIndex. This is the slot that you dragged the item from.

        string tempName = eventData.pointerDrag.transform.parent.name;

        if(tempName.Contains("i_"))
        {
            if(mainInventory.gameObject.transform.parent.gameObject.activeSelf == true)
            {
                droppedItem = mainInventory.slotScripts[eventData.pointerDrag.GetComponent<ItemDrag>().transform.parent.GetSiblingIndex()]; //NEED TO MAKE IT SO THAT THE INVENTORY IN THE INVENTORY SCREEN CAN MOVE ITEM INDEPENDENT OF THE LOADOUT INVENTORY
            }
            else
            {
                droppedItem = inv.slotScripts[eventData.pointerDrag.GetComponent<ItemDrag>().transform.parent.GetSiblingIndex()];
            }


            for(int i = 0; i < mainInventory.slotScripts.Count; i++)
            {
                if(mainInventory.slotScripts[i].item == droppedItem.item)
                {
                    dupeDroppedItem = mainInventory.slotScripts[i];
                }
            }

            print("The Dropped Item is: " + droppedItem);

            //if the object that the slot is dropped on is itself, do nothing

            if (eventData.pointerDrag.transform.parent.name == gameObject.name)
            {
                print("The item was dropped over itself");
                return;
            }


            if(gameObject.name == "Delete Slot")
            {
                //add value of item to the currency
                if(droppedItem.item.GetComponent<Item>().itemStats.displayName.Contains("Altered"))
                {
                    invManager.currency1 += droppedItem.item.GetComponent<Item>().itemStats.sellPrice;
                    mainInventory.datumCurrency += droppedItem.item.GetComponent<Item>().itemStats.sellPrice;
                }
                else
                {
                    invManager.currency += droppedItem.item.GetComponent<Item>().itemStats.sellPrice;
                    mainInventory.creditCurrency += droppedItem.item.GetComponent<Item>().itemStats.sellPrice;
                }

                //remove the item from the players inventory and the inventory manager
                foreach(Transform child in invManager.gameObject.GetComponentInChildren<Transform>())
                {
                    if(child.gameObject.GetInstanceID() == droppedItem.item.GetInstanceID()) //find an exact copy of this item and destroy it from the inventory managers child objects
                    {
                        Destroy(child.gameObject);
                        break;
                    }
                }
                invManager.allPickedUpObjects.Remove(droppedItem.item);
                invManager.OnItemMove(droppedItem.item, false);

                //null values on the item in the current menu
                droppedItem.item = null;
                droppedItem.icon = null;
                droppedItem.type = null;
                droppedItem.description = null;

                int tempIndex = mainInventory.slotScripts.IndexOf(droppedItem);

                //null values on the item in the loadout menu
                inv.slotScripts[tempIndex].item = null;
                inv.slotScripts[tempIndex].icon = null;
                inv.slotScripts[tempIndex].type = null;
                inv.slotScripts[tempIndex].description = null;


                for (int i = 0; i < droppedItem.bonusStats.Count; i++)
                {
                    droppedItem.bonusStats[i] = 0;
                }

                mainInventory.UpdateSlotUI();
                inv.UpdateSlotUI();
            }

            //if the slot being dropped on is empty, place the dropped item in that slot and then update the UI
            if (gameObject.name.Contains("i_")) //item is moving from one inventory slot to another
            {
                if (mainInventory.gameObject.transform.parent.gameObject.activeSelf == true)
                {
                    if (mainInventory.slotScripts[transform.GetSiblingIndex()].item == null)
                    {
                        print("The slot that was dropped on is empty");

                        tempTarget = mainInventory.slotScripts[transform.GetSiblingIndex()];

                        print("Temp target is: " + tempTarget);

                        tempTarget.item = droppedItem.item;
                        tempTarget.item.GetComponent<Item>().itemStats.equipped = false;
                        tempTarget.icon = droppedItem.icon;
                        tempTarget.type = droppedItem.type;
                        tempTarget.description = droppedItem.description;
                        tempTarget.ID = droppedItem.ID;

                        for (int i = 0; i < tempTarget.bonusStats.Count; i++)
                        {
                            tempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                        }


                        droppedItem.item = null;
                        droppedItem.icon = null;
                        droppedItem.type = null;
                        droppedItem.description = null;

                        for (int i = 0; i < droppedItem.bonusStats.Count; i++)
                        {
                            droppedItem.bonusStats[i] = 0;
                        }

                        mainInventory.UpdateSlotUI();
                        invEquip.UpdateSlotUI();
                    }
                    else
                    {
                        print("The slot that was dropped on is not empty");
                        return;
                    }
                }
                else
                {
                    if (inv.slotScripts[transform.GetSiblingIndex()].item == null)
                    {
                        print("The slot that was dropped on is empty");

                        tempTarget = inv.slotScripts[transform.GetSiblingIndex()];

                        print("Temp target is: " + tempTarget);

                        tempTarget.item = droppedItem.item;
                        tempTarget.item.GetComponent<Item>().itemStats.equipped = false;
                        tempTarget.icon = droppedItem.icon;
                        tempTarget.type = droppedItem.type;
                        tempTarget.description = droppedItem.description;
                        tempTarget.ID = droppedItem.ID;

                        for (int i = 0; i < tempTarget.bonusStats.Count; i++)
                        {
                            tempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                        }


                        droppedItem.item = null;
                        droppedItem.icon = null;
                        droppedItem.type = null;
                        droppedItem.description = null;

                        for (int i = 0; i < droppedItem.bonusStats.Count; i++)
                        {
                            droppedItem.bonusStats[i] = 0;
                        }

                        inv.UpdateSlotUI();
                        invEquip.UpdateSlotUI();
                    }
                    else
                    {
                        print("The slot that was dropped on is not empty");
                        return;
                    }
                }
            }
            else if (gameObject.name.Contains("e_"))  //item is moving from inventory to equipment
            {
                if (invEquip.slotScripts[transform.GetSiblingIndex()].item == null)
                {
                    print("The slot that was dropped on is empty");

                    tempTarget = invEquip.slotScripts[transform.GetSiblingIndex()];

                    print("Temp target is: " + tempTarget);

                    tempTarget.item = droppedItem.item;
                    tempTarget.item.GetComponent<Item>().itemStats.equipped = true;
                    tempTarget.icon = droppedItem.icon;
                    tempTarget.type = droppedItem.type;
                    tempTarget.description = droppedItem.description;
                    tempTarget.ID = droppedItem.ID;

                    for(int i = 0; i < tempTarget.bonusStats.Count; i++) //for each stat on the temptarget, set the stats on the droppeditem
                    {
                        tempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                    }

                    //add the item to the inventory manager
                    if(hubStats.leadStats)
                        invManager.leadEquipment.Add(tempTarget.item);
                    if (hubStats.blueStats)
                        invManager.blueEquipment.Add(tempTarget.item);
                    if (hubStats.greenStats)
                        invManager.greenEquipment.Add(tempTarget.item);
                    if (hubStats.orangeStats)
                        invManager.orangeEquipment.Add(tempTarget.item);

                    //remove the item from the inventory lists
                    invManager.OnItemMove(droppedItem.item, false);

                    //set the dropped item stats to null
                    droppedItem.item = null;
                    droppedItem.icon = null;
                    droppedItem.type = null;
                    droppedItem.description = null;
                    dupeDroppedItem.item = null;
                    dupeDroppedItem.icon = null;
                    dupeDroppedItem.type = null;
                    dupeDroppedItem.description = null;

                    //set the dropped item stats to 0
                    for(int i = 0; i < droppedItem.bonusStats.Count; i++)
                    {
                        droppedItem.bonusStats[i] = 0;
                        dupeDroppedItem.bonusStats[i] = 0;
                    }

                    //updates the slots so that they show the correct images
                    inv.UpdateSlotUI();
                    invEquip.UpdateSlotUI();
                    mainInventory.UpdateSlotUI();

                    //updates the stats for the manager and the loadout screen
                    hubStats.ChangeBonuses(tempTarget, true);
                    //sets a bool for what bot owns the item
                    invManager.GetEquipped(tempTarget);
                }
                else
                {
                    print("The slot that was dropped on is not empty");
                    return;
                }
            }
        }
        else if(tempName.Contains("e_"))
        {
            droppedItem = invEquip.slotScripts[eventData.pointerDrag.GetComponent<ItemDrag>().transform.parent.GetSiblingIndex()];

            print("The Dropped Item is: " + droppedItem);

            if (eventData.pointerDrag.transform.parent.name == gameObject.name)
            {
                print("The item was dropped over itself");
                return;
            }

            if(gameObject.name.Contains("i_")) //item is moving from equipment to inventory
            {
                if (inv.slotScripts[transform.GetSiblingIndex()].item == null)
                {
                    print("The slot that was dropped on is empty");

                    tempTarget = inv.slotScripts[transform.GetSiblingIndex()];
                    int targetIndex = inv.slotScripts.IndexOf(tempTarget);
                    dupeTempTarget = mainInventory.slotScripts[targetIndex];

                    print("Temp target is: " + tempTarget);

                    tempTarget.item = droppedItem.item;
                    dupeTempTarget.item = droppedItem.item;
                    tempTarget.icon = droppedItem.icon;
                    dupeTempTarget.icon = droppedItem.icon;
                    tempTarget.type = droppedItem.type;
                    dupeTempTarget.type = droppedItem.type;
                    tempTarget.description = droppedItem.description;
                    dupeTempTarget.description = droppedItem.description;
                    tempTarget.ID = droppedItem.ID;
                    dupeTempTarget.ID = droppedItem.ID;

                    for (int i = 0; i < tempTarget.bonusStats.Count; i++)
                    {
                        tempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                        dupeTempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                    }

                    tempTarget.item.GetComponent<Item>().itemStats.equipped = false;
                    dupeTempTarget.item.GetComponent<Item>().itemStats.equipped = false;
                    tempTarget.item.GetComponent<Item>().itemStats.leadOwns = false;
                    dupeTempTarget.item.GetComponent<Item>().itemStats.leadOwns = false;
                    tempTarget.item.GetComponent<Item>().itemStats.blueOwns = false;
                    dupeTempTarget.item.GetComponent<Item>().itemStats.blueOwns = false;
                    tempTarget.item.GetComponent<Item>().itemStats.greenOwns = false;
                    dupeTempTarget.item.GetComponent<Item>().itemStats.greenOwns = false;
                    tempTarget.item.GetComponent<Item>().itemStats.orangeOwns = false;
                    dupeTempTarget.item.GetComponent<Item>().itemStats.orangeOwns = false;

                    if (hubStats.leadStats)
                        invManager.leadEquipment.Remove(tempTarget.item);
                    if (hubStats.blueStats)
                        invManager.blueEquipment.Remove(tempTarget.item);
                    if (hubStats.greenStats)
                        invManager.greenEquipment.Remove(tempTarget.item);
                    if (hubStats.orangeStats)
                        invManager.orangeEquipment.Remove(tempTarget.item);

                    invManager.OnItemMove(droppedItem.item, true);

                    droppedItem.item = null;
                    droppedItem.icon = null;
                    droppedItem.type = null;
                    droppedItem.description = null;

                    for (int i = 0; i < droppedItem.bonusStats.Count; i++)
                    {
                        droppedItem.bonusStats[i] = 0;
                    }

                    inv.UpdateSlotUI();
                    invEquip.UpdateSlotUI();
                    mainInventory.UpdateSlotUI();
                    hubStats.ChangeBonuses(tempTarget, false);
                }
                else
                {
                    print("The slot that was dropped on is not empty");
                    return;
                }
            }
            else if (gameObject.name.Contains("e_")) //item is moving from one equipment slot to another
            {
                if (invEquip.slotScripts[transform.GetSiblingIndex()].item == null)
                {
                    print("The slot that was dropped on is empty");

                    tempTarget = invEquip.slotScripts[transform.GetSiblingIndex()];

                    print("Temp target is: " + tempTarget);

                    tempTarget.item = droppedItem.item;
                    tempTarget.item.GetComponent<Item>().itemStats.equipped = true;
                    tempTarget.icon = droppedItem.icon;
                    tempTarget.type = droppedItem.type;
                    tempTarget.description = droppedItem.description;
                    tempTarget.ID = droppedItem.ID;

                    for (int i = 0; i < tempTarget.bonusStats.Count; i++)
                    {
                        tempTarget.bonusStats[i] = droppedItem.bonusStats[i];
                    }

                    droppedItem.item = null;
                    droppedItem.icon = null;
                    droppedItem.type = null;
                    droppedItem.description = null;

                    for (int i = 0; i < droppedItem.bonusStats.Count; i++)
                    {
                        droppedItem.bonusStats[i] = 0;
                    }

                    inv.UpdateSlotUI();
                    invEquip.UpdateSlotUI();
                    hubStats.ChangeBonuses(tempTarget, true);
                    hubStats.ChangeBonuses(tempTarget, false);
                }
                else
                {
                    print("The slot that was dropped on is not empty");
                    return;
                }
            }
        }
    }
}
