using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileData
{
    public List<string> fileNames = new List<string>();

    public int creditCount;
    public int datumCount;

    public FileData(SaveFileManager saveMgr)
    {
        for(int i = 0; i < saveMgr.fileNames.Count; i++)
        {
            fileNames.Add(saveMgr.fileNames[i]);
        }

        creditCount = saveMgr.creditCount;
        datumCount = saveMgr.datumCount;
    }
}
