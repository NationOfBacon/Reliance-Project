using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> itemsForReference = new List<GameObject>();
    public List<GameObject> allPickedUpObjects = new List<GameObject>(); //keeps holding items between scenes, duplicates possible
    public List<GameObject> loreObjects = new List<GameObject>(); //keeps holding lore items between scenes, duplicates not possible
    public List<GameObject> leadEquipment = new List<GameObject>();
    public List<GameObject> blueEquipment = new List<GameObject>();
    public List<GameObject> greenEquipment = new List<GameObject>();
    public List<GameObject> orangeEquipment = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>(); //holds current items that are in slots for the loadout inventory
    public List<int> tempRewards = new List<int>();
    public float currencyLost;
    public int currency;
    public int currency1;
    public bool addCurrency = false;
    public bool loadDataOnLevelLoad = false;
    public bool resetDataForNewGame = false;

    [HideInInspector]
    public Inventory inv;
    [HideInInspector]
    public Inventory inv2;
    [HideInInspector]
    public Inventory equipInv;
    [HideInInspector]
    public Inventory sellInv;

    private HUBStats hubStats;
    private HUBTracker hubTracker;
    private SaveFileManager saveMgr;
    private UIManager uiMgr;
    private Lore lore;

    private void Awake()
    {
        uiMgr = GetComponent<UIManager>();
        hubTracker = GetComponent<HUBTracker>();
        saveMgr = GetComponent<SaveFileManager>();
    }

    public void DeleteChildObjects() //called to delete all children objects from under the persistent object
    {
        for(int i = 0; i < allPickedUpObjects.Count; i++)
        {
            allPickedUpObjects[i].SetActive(true);
        }

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != this.gameObject)
                Destroy(child.gameObject);
        }

        allPickedUpObjects.Clear();
        inventoryItems.Clear();
    }

    public void LoadInventoryData(string fileName)
    {
        InventoryData data = SaveSystem.LoadInventoryData(fileName);

        //Clear all lists before adding the saved items to them
        leadEquipment.Clear();
        blueEquipment.Clear();
        greenEquipment.Clear();
        orangeEquipment.Clear();
        allPickedUpObjects.Clear();
        inventoryItems.Clear();
        loreObjects.Clear();

        for (int i = 0; i < 5; i++) //for each list from the data
        {
            List<GameObject> currentList = new List<GameObject>(); //holds prefab objects that have not been spawned into the scene
            List<GameObject> objectsList = new List<GameObject>(); //holds gameobject prefabs that have been spawned under the manager gameobject as children
            List<InventoryData.ItemInfo> infoList = new List<InventoryData.ItemInfo>();

            infoList.Clear();

            if (i == 0)
                infoList = data.leadEquipment.items;
            else if (i == 1)
                infoList = data.blueEquipment.items;
            else if (i == 2)
                infoList = data.greenEquipment.items;
            else if (i == 3)
                infoList = data.orangeEquipment.items;
            else if (i == 4)
                infoList = data.loadoutInventory.items;

            for (int y = 0; y < infoList.Count; y++) //for each item in the current list from the data
            {
                for(int x = 0; x < itemsForReference.Count; x++) //for each item in the reference items list
                {
                    if (infoList[y].itemID == itemsForReference[x].GetComponent<Item>().itemStats.ID) //if the two items have matching IDs
                    {
                        currentList.Add(itemsForReference[x]);
                    }
                }
            }

            for (int y = 0; y < currentList.Count; y++)
            {
                objectsList.Add(Instantiate(currentList[y] as GameObject, gameObject.transform));
            }

            if (i == 0)
            {
                for (int y = 0; y < objectsList.Count; y++)
                {
                    leadEquipment.Add(objectsList[y]);
                    allPickedUpObjects.Add(objectsList[y]);
                    leadEquipment[y].GetComponent<Item>().itemStats.leadOwns = true;
                    leadEquipment[y].GetComponent<Item>().itemStats.equipped = true;
                    equipInv.ItemRetrieve(leadEquipment[y].GetComponent<Item>().itemStats);
                }
            }
            else if (i == 1)
            {
                for (int y = 0; y < objectsList.Count; y++)
                {
                    blueEquipment.Add(objectsList[y]);
                    allPickedUpObjects.Add(objectsList[y]);
                    blueEquipment[y].GetComponent<Item>().itemStats.blueOwns = true;
                    blueEquipment[y].GetComponent<Item>().itemStats.equipped = true;
                    equipInv.ItemRetrieve(blueEquipment[y].GetComponent<Item>().itemStats);
                }
            }
            else if (i == 2)
            {
                for (int y = 0; y < objectsList.Count; y++)
                {
                    greenEquipment.Add(objectsList[y]);
                    allPickedUpObjects.Add(objectsList[y]);
                    greenEquipment[y].GetComponent<Item>().itemStats.greenOwns = true;
                    greenEquipment[y].GetComponent<Item>().itemStats.equipped = true;
                    equipInv.ItemRetrieve(greenEquipment[y].GetComponent<Item>().itemStats);
                }
            }
            else if (i == 3)
            {
                for (int y = 0; y < objectsList.Count; y++)
                {
                    orangeEquipment.Add(objectsList[y]);
                    allPickedUpObjects.Add(objectsList[y]);
                    orangeEquipment[y].GetComponent<Item>().itemStats.orangeOwns = true;
                    orangeEquipment[y].GetComponent<Item>().itemStats.equipped = true;
                    equipInv.ItemRetrieve(orangeEquipment[y].GetComponent<Item>().itemStats);
                }
            }
            else if (i == 4)
            {
                for(int y = 0; y < objectsList.Count; y++)
                {
                    allPickedUpObjects.Add(objectsList[y]);
                    inventoryItems.Add(objectsList[y]);
                    inv.ItemRetrieve(inventoryItems[y].GetComponent<Item>().itemStats);
                    inv2.ItemRetrieve(inventoryItems[y].GetComponent<Item>().itemStats);
                }
            }
        }

        inv.UpdateSlotUI();
        inv2.UpdateSlotUI();
        equipInv.UpdateSlotUI();
        sellInv.UpdateSlotUI();

        currency = data.credits;
        currency1 = data.datum;
    }

    public void OnItemMove(GameObject movedItem, bool adding) //when an item is moved to a different slot from the slot script, this will modify the lists that hold the items
    {
        if(adding)
             inventoryItems.Add(movedItem);
        else
             inventoryItems.Remove(movedItem);
    }

    public void OnLevelLoad(Scene scene)
    {
        if (scene.name == "HUB scene")
        {
            inv = GameObject.Find("HUBUI/Inventory Panel/Grid Layout").GetComponent<Inventory>();
            inv2 = GameObject.Find("HUBUI/Loadout Panel/Inventory Grid").GetComponent<Inventory>();
            equipInv = GameObject.Find("HUBUI/Loadout Panel/Equipment Grid").GetComponent<Inventory>();
            sellInv = GameObject.Find("HUBUI/Inventory Panel/Sell Inv").GetComponent<Inventory>();
            hubStats = GameObject.Find("HUBUI/Loadout Panel/Stats Backing").GetComponent<HUBStats>();
            lore = GameObject.Find("HUBUI/Information Panel").GetComponent<Lore>();

            uiMgr.HUBUISet();

            if (allPickedUpObjects.Count > 0)
            {
                for (int i = 0; i < allPickedUpObjects.Count; i++)
                {
                    Item item = allPickedUpObjects[i].GetComponent<Item>();

                    if (item.itemStats.equipped == true) //check if the item is equipped and then if the lead bot has it. Since the leadbot will always show when first opening the loadout screen, no other bots need to be checked
                    {
                        if (item.itemStats.leadOwns)
                            equipInv.ItemRetrieve(item.itemStats);
                    }
                    else
                    {
                        inv.ItemRetrieve(item.itemStats);
                        inv2.ItemRetrieve(item.itemStats);
                    }
                }
            }

            if (loreObjects.Count > 0)
            {
                for (int i = 0; i < loreObjects.Count; i++)
                {
                    Item item = loreObjects[i].GetComponent<Item>();

                    lore.ItemRetrieve(item.itemStats);
                }
            }

            if(loadDataOnLevelLoad) //called when a button is pressed that sets the bool false, loads save data when the level loads
            {
                hubTracker.LoadDataFromFile(saveMgr.saveFileName);
                LoadInventoryData(saveMgr.saveFileName);

                loadDataOnLevelLoad = false;
            }

            if(resetDataForNewGame) //called when a button is pressed that sets the bool false, clears save data and then updates the UI when the level loads
            {
                Debug.Log("Reseting data for new game");

                hubTracker.ClearAllSaveData();
                ClearAllSaveData();
                ClearEquipment();

                inv.UpdateSlotUI();
                inv2.UpdateSlotUI();
                equipInv.UpdateSlotUI();
                sellInv.UpdateSlotUI();

                resetDataForNewGame = false;
            }
        }
    }

    public void ClearAllSaveData() //called to clear all data from lists and slots for the inventory. Also resets currency
    {
        leadEquipment.Clear();
        blueEquipment.Clear();
        greenEquipment.Clear();
        orangeEquipment.Clear();
        allPickedUpObjects.Clear();
        inventoryItems.Clear();
        loreObjects.Clear();

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != this.gameObject)
                Destroy(child.gameObject);
        }

        for (int y = 0; y < 2; y++)
        {
            List<Slot> currentSlots = new List<Slot>();

            if (y == 0)
                currentSlots = inv.slotScripts;
            else if (y == 1)
                currentSlots = inv2.slotScripts;

            for (int i = 0; i < currentSlots.Count; i++)
            {
                Slot slot = currentSlots[i];

                slot.item = null;
                slot.icon = null;
                slot.ID = 0;
                slot.type = null;
                slot.description = null;
                slot.empty = true;

                inv.UpdateSlotUI();
                inv2.UpdateSlotUI();

                for (int x = 0; x < slot.bonusStats[x];)
                {
                    slot.bonusStats[x] = 0;
                }
            }

            if (y == 0)
                inv.slotScripts = currentSlots;
            else if (y == 1)
                inv2.slotScripts = currentSlots;
        }

        currency = 0;
        currency1 = 0;
    }

    public void ModifyCurrency()
    {
        if(addCurrency)
        {
            currency += tempRewards[0];
            currency1 += tempRewards[1];
            if (currencyLost < 0)
                currency += Mathf.FloorToInt(currencyLost);

            if(currency < 0)
                currency = 0;

            if (currency1 < 0)
                currency1 = 0;

            inv.creditCurrency = currency;
            inv.datumCurrency = currency1;

            currencyLost = 0;
            tempRewards.Clear();
        }
        else
        {
            tempRewards.Clear();
            currencyLost = 0;
        }
    }

    public void GetEquipped(Slot targetSlot)
    {
        for (int i = 0; i < allPickedUpObjects.Count; i++)
        {
            if(targetSlot.item.name == allPickedUpObjects[i].name)
            {
                allPickedUpObjects[i].GetComponent<Item>().itemStats.equipped = true;

                if(hubStats.leadStats)
                {
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.leadOwns = true;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.blueOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.greenOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.orangeOwns = false;
                }

                if (hubStats.blueStats)
                {
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.leadOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.blueOwns = true;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.greenOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.orangeOwns = false;
                }

                if (hubStats.greenStats)
                {
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.leadOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.blueOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.greenOwns = true;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.orangeOwns = false;
                }

                if (hubStats.orangeStats)
                {
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.leadOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.blueOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.greenOwns = false;
                    allPickedUpObjects[i].GetComponent<Item>().itemStats.orangeOwns = true;
                }
            }
        }
    }

    public void UpdateEquipment() //called when the bool for what bot is selected changes. Changes what items are displayed in the equipment inventory.
    {
        ClearEquipment();

        if (hubStats.leadStats)
        {
            if (leadEquipment.Count > 0)
            {
                ClearEquipment();

                for (int x = 0; x < leadEquipment.Count; x++)
                {
                    Item botItem = leadEquipment[x].GetComponent<Item>();

                    equipInv.ItemRetrieve(botItem.itemStats);
                }

                equipInv.UpdateSlotUI();
            }
        }

        if (hubStats.blueStats)
        {
            if (blueEquipment.Count > 0)
            {
                ClearEquipment();

                for (int x = 0; x < blueEquipment.Count; x++)
                {
                    Item botItem = blueEquipment[x].GetComponent<Item>();

                    equipInv.ItemRetrieve(botItem.itemStats);
                }

                equipInv.UpdateSlotUI();
            }
        }

        if (hubStats.greenStats)
        {
            if (greenEquipment.Count > 0)
            {
                ClearEquipment();

                for (int x = 0; x < greenEquipment.Count; x++)
                {
                    Item botItem = greenEquipment[x].GetComponent<Item>();

                    equipInv.ItemRetrieve(botItem.itemStats);
                }

                equipInv.UpdateSlotUI();
            }
        }

        if (hubStats.orangeStats)
        {
            if (orangeEquipment.Count > 0)
            {
                ClearEquipment();

                for (int x = 0; x < orangeEquipment.Count; x++)
                {
                    Item botItem = orangeEquipment[x].GetComponent<Item>();

                    equipInv.ItemRetrieve(botItem.itemStats);
                }

                equipInv.UpdateSlotUI();
            }
        }
    }

    public void ClearEquipment()
    {
        for (int i = 0; i < equipInv.slotScripts.Count; i++)
        {
            Slot slot = equipInv.slotScripts[i];

            slot.item = null;
            slot.icon = null;
            slot.ID = 0;
            slot.type = null;
            slot.description = null;
            slot.empty = true;

            equipInv.UpdateSlotUI();

            for (int x = 0; x < slot.bonusStats[x];)
            {
                slot.bonusStats[x] = 0;
            }
        }
    }
}
