using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    public string saveFileName;

    public List<string> fileNames = new List<string>();
    public int creditCount; //stored but not retrieved anywhere
    public int datumCount; //stored but not retrieved anywhere

    private InventoryManager manager;
    private HUBTracker tracker;
    private GameObject saveGameNotification;

    private void Awake()
    {
        manager = GetComponent<InventoryManager>();
        tracker = GetComponent<HUBTracker>();
    }

    private void Start()
    {
        FileData data = SaveSystem.LoadFileData(); //when the game starts get the file names from the save data

        fileNames = data.fileNames;
    }

    public void OnLevelLoad(Scene currentScene)
    {
        if(currentScene.name == "HUB scene")
        {
            saveGameNotification = GameObject.Find("HUBUI/Save Notification");
            saveGameNotification.SetActive(false);
            SendDataToSaveSystem();
            saveGameNotification.GetComponent<SaveNotificationControl>().RunAnimation();
        }
    }

    public void SendDataToSaveSystem()
    {
        SaveSystem.SaveData(tracker, manager, saveFileName);

        bool duplicateFile = false;

        for(int i = 0; i < fileNames.Count; i++) //if any of the file names that the list already holds match the one thats being added, dont add it
        {
            if(fileNames[i] == saveFileName)
            {
                duplicateFile = true;
            }
        }

        if (duplicateFile == false)
            fileNames.Add(saveFileName);

        creditCount = manager.currency;
        datumCount = manager.currency1;

        SaveSystem.SaveFileData(this);
    }

    public void SaveOnlyFileData()
    {
        SaveSystem.SaveFileData(this);
    }
}
