using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveFileName : MonoBehaviour
{
    public TextMeshProUGUI textField;
    private SaveFileManager saveMgr;

    private void Awake()
    {
        saveMgr = GameObject.Find("Persistent Object").GetComponent<SaveFileManager>();
    }

    public void SaveName()
    {
        saveMgr.saveFileName = textField.text;
    }
}
