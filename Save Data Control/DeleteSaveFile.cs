using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveFile : MonoBehaviour
{
    SaveFileManager saveMgr;
    DataPanelInfo panelInfo;
    private string fileName;

    private void Awake()
    {
        saveMgr = GameObject.Find("Persistent Object").GetComponent<SaveFileManager>();
        panelInfo = GetComponent<DataPanelInfo>();
    }

    private void Start()
    {
        fileName = panelInfo.fileNameText.text;
    }

    public void OnDeleteSave()
    {
        for(int i = 0; i < saveMgr.fileNames.Count; i++)
        {
            if(saveMgr.fileNames[i] == fileName)
            {
                saveMgr.fileNames.RemoveAt(i);
                SaveSystem.DeleteSave(fileName);
                saveMgr.SaveOnlyFileData();
            }
        }

        Destroy(gameObject);
    }
}
