using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSave : MonoBehaviour
{
    HUBTracker tracker;
    InventoryManager manager;
    SaveFileManager saveInfo;

    private void Start()
    {
        tracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        manager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        saveInfo = GameObject.Find("Persistent Object").GetComponent<SaveFileManager>();
    }

    public void SaveOnPress()
    {
        saveInfo.SendDataToSaveSystem();
    }

    public void LoadOnPress()
    {
        tracker.LoadDataFromFile(saveInfo.saveFileName);
        manager.LoadInventoryData(saveInfo.saveFileName);
    }

    public void SetOnManagerLevelLoad() //called from the main menu to have the Inventory manager load data after the HUB is loaded to avoid missing references
    {
        manager.loadDataOnLevelLoad = true;
    }

    public void ClearDataForNewGame() //called when pressing the new game button on the main menu, clears all data related to saves
    {
        manager.resetDataForNewGame = true;
    }

    public void ClearInventoryChildren()
    {
        manager.DeleteChildObjects();
    }
}
