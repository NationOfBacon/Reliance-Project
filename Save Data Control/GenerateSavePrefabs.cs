using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSavePrefabs : MonoBehaviour
{
    public GameObject savePrefab;
    private SaveFileManager saveMgr;

    private void Awake()
    {
        saveMgr = GameObject.Find("Persistent Object").GetComponent<SaveFileManager>();
    }

    private void Start()
    {
        OnShowPanel();
    }

    public void OnShowPanel()
    {
        if(saveMgr.fileNames.Count > 0)
        {
            for(int i = 0; i < saveMgr.fileNames.Count; i++)
            {
                GameObject tempObject = Instantiate(savePrefab as GameObject, transform);

                tempObject.GetComponent<DataPanelInfo>().fileNameText.text = saveMgr.fileNames[i];
            }
        }
        else
        {
            Debug.Log("No save files have been made so no save prefabs were generated");
        }
    }
}
