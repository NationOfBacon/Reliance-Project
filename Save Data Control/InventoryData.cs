using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    [System.Serializable]
    public struct ItemInfo
    {
        public int itemID;
        public int slotPosition;
        public bool equipped;
        public int ownerID;
    };

    [System.Serializable]
    public struct Equipment
    {
        public List<ItemInfo> items;
    };

    public Equipment leadEquipment = new Equipment();
    public Equipment blueEquipment = new Equipment();
    public Equipment greenEquipment = new Equipment();
    public Equipment orangeEquipment = new Equipment();
    public Equipment loadoutInventory = new Equipment();

    public int credits;
    public int datum;

    public InventoryData(InventoryManager manager)
    {
        for(int i = 0; i < 5; i++) //for each inventory being tracked
        {
            List<GameObject> currentManagerList = new List<GameObject>();
            List<ItemInfo> currentInv = new List<ItemInfo>();

            currentInv.Clear();

            if (i == 0)
                currentManagerList = manager.leadEquipment;
            else if (i == 1)
                currentManagerList = manager.blueEquipment;
            else if (i == 2)
                currentManagerList = manager.greenEquipment;
            else if (i == 3)
                currentManagerList = manager.orangeEquipment;
            else if (i == 4)
                currentManagerList = manager.inventoryItems;

            if(currentManagerList.Count != 0)
            {
                for (int y = 0; y < currentManagerList.Count; y++) //for each item in the current list, set the item information
                {
                    Item currentItem = currentManagerList[y].GetComponent<Item>();

                    ItemInfo newItem = new ItemInfo
                    {
                        itemID = currentItem.itemStats.ID,
                        slotPosition = y,
                        equipped = currentItem.itemStats.equipped
                    };

                    if (currentItem.itemStats.leadOwns)
                        newItem.ownerID = 0;
                    else if (currentItem.itemStats.blueOwns)
                        newItem.ownerID = 1;
                    else if (currentItem.itemStats.greenOwns)
                        newItem.ownerID = 2;
                    if (currentItem.itemStats.orangeOwns)
                        newItem.ownerID = 3;

                    currentInv.Add(newItem);
                }
            }

            if (i == 0)
                leadEquipment.items = currentInv;
            else if (i == 1)
                blueEquipment.items = currentInv;
            else if (i == 2)
                greenEquipment.items = currentInv;
            else if (i == 3)
                orangeEquipment.items = currentInv;
            else if (i == 4)
                loadoutInventory.items = currentInv;
        }

        credits = manager.currency;
        datum = manager.currency1;

        Debug.Log(leadEquipment.items.Count + " " + blueEquipment.items.Count + " " + greenEquipment.items.Count + " " + orangeEquipment.items.Count + " " + loadoutInventory.items.Count);
    }
}
