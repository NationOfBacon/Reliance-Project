using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTargetSave : MonoBehaviour
{
    private string fileName;
    private DataPanelInfo panelInfo;
    private SaveFileManager saveMgr;

    private void Awake()
    {
        panelInfo = GetComponent<DataPanelInfo>();
        saveMgr = GameObject.Find("Persistent Object").GetComponent<SaveFileManager>();
    }

    private void Start()
    {
        fileName = panelInfo.fileNameText.text;
    }

    public void OnLoadTargetSave() //change the file name held by the save file manager so that when it goes to load the save files, it uses the correct name
    {
        saveMgr.saveFileName = fileName;
    }
}
