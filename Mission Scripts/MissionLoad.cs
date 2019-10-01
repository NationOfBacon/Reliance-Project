using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionLoad : MonoBehaviour
{
    public string missionName;
    public int reward1;
    public int reward2;
    private InventoryManager invManager;
    private GameManager gameManager;

    private void Awake()
    {
        invManager = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
    }

    public void LoadMission() //gets rewards off of mission and sends them to the inventory manager. After sending, loads mission
    {
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if(child.gameObject.name == "Reward #")
            {
                string tempReward = child.GetComponent<TextMeshProUGUI>().text;
                reward1 = int.Parse(tempReward);
                invManager.tempRewards.Add(reward1);
            }

            if (child.gameObject.name == "Reward # 1")
            {
                string tempReward = child.GetComponent<TextMeshProUGUI>().text;
                reward2 = int.Parse(tempReward);
                invManager.tempRewards.Add(reward2);
            }
        }

        SceneManager.LoadScene(missionName);
    }

    public void GetDifficulty(TextMeshProUGUI targetText) //Gets the mission difficulty text off of the mission
    {
        gameManager.difficulty = targetText.text;
    }

    public void GetMissionTitle(TextMeshProUGUI targetText)
    {
        gameManager.missionName = targetText.text;
    }
}
