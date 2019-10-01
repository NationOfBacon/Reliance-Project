using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMissionDescription : MonoBehaviour //attaches to StartUI to get the mission description
{
    private GameManager gameMgr;

    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        gameMgr = GameObject.Find("Persistent Object").GetComponent<GameManager>();
    }

    private void Start()
    {
        string missionName = gameMgr.missionName;

        if (missionName == "Defend")
        {
            descriptionText.text = "Stop the enemy from destroying our structure";
        }
        else if (missionName == "Protect")
        {
            descriptionText.text = "Allied bots are deployed in the area, protect them and destroy all enemies";
        }
        else if (missionName == "Destroy")
        {
            descriptionText.text = "Destroy the enemy structure in the area";
        }
        else if (missionName == "Assault")
        {
            descriptionText.text = "Take over an enemy held area";
        }
        else if (missionName == "Hack")
        {
            descriptionText.text = "Hack into the enemy terminal to gather data";
        }
        else if (missionName == "Kill")
        {
            descriptionText.text = "Destroy all enemy bots in the area";
        }
    }
}
