using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionSelect : MonoBehaviour
{
    private List<string> missionNames = new List<string>() { "Assault", "Defend", "Kill", "Protect", "Destroy", "Hack"};
    private List<string> missionDetails = new List<string>() { "Take over an enemy held area", "Stop the enemy from destroying our structure", "Destroy all enemy bots in the area", "Allied bots are deployed in the area, protect them and destroy all enemies", "Destroy the enemy structure in the area", "Hack into the enemy terminal to gather data"};
    private List<string> factionNames = new List<string>() { "Tannerick", "Foundry", "Intelligent Applications", "Welders", "Orbital Acquisions", "Holy Federation", "Reclaimers" };
    private List<string> difficultySetting = new List<string>() { "Easy", "Normal", "Hard", "Insane" };

    public GameObject missionPrefab;
    private TextMeshProUGUI missionName;
    private TextMeshProUGUI missionDesc;
    private TextMeshProUGUI factionName;
    private TextMeshProUGUI difficulty;
    private TextMeshProUGUI rewards1;
    private int rewardAmt;
    private TextMeshProUGUI rewards2;
    private int rewardAmt1;
    private HUBTracker hubTracker;
    private InventoryManager invMgr;
    private int currentCash;

    private bool generated;

    void Start()
    {
        generated = false;
        hubTracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        invMgr = GameObject.Find("Persistent Object").GetComponent<InventoryManager>();
        currentCash = invMgr.currency;
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "HUB scene")
        {
            currentCash = invMgr.currency;

            if (generated == false)
            {
                int fourthRandom = Random.Range(3, 8);
                for (int i = 0; i < fourthRandom; i++) //create the i variable and increment it by 1 each time it is below the random count untill it is the same as the random count.
                {
                    Instantiate(missionPrefab as GameObject, this.gameObject.transform, false); //spawn the number of random mission prefabs
                }

                generated = true;

                foreach (GameObject child in GameObject.FindGameObjectsWithTag("Mission")) //set all the variables to fill in the mission prefabs
                {

                    missionName = child.transform.Find("Mission Title").GetComponent<TextMeshProUGUI>();
                    missionDesc = child.transform.Find("Mission Description").GetComponent<TextMeshProUGUI>();
                    factionName = child.transform.Find("Faction Title").GetComponent<TextMeshProUGUI>();
                    difficulty = child.transform.Find("Difficulty").GetComponent<TextMeshProUGUI>();
                    rewards1 = child.transform.Find("Reward #").GetComponent<TextMeshProUGUI>();
                    rewards2 = child.transform.Find("Reward # 1").GetComponent<TextMeshProUGUI>();

                    int random = Random.Range(0, missionNames.Count);
                    missionName.text = missionNames[random];
                    missionDesc.text = missionDetails[random];

                    int secondRandom = Random.Range(0, factionNames.Count);
                    factionName.text = factionNames[secondRandom];

                    float thirdRandom = 0; //thirdRandom is used to define the difficulty of a mission and decides the rewards that are given

                    if(hubTracker.allBots[0].level >= 7 || hubTracker.allBots[1].level >= 7 || hubTracker.allBots[2].level >= 7 || hubTracker.allBots[3].level >= 7) //if any bots level is greater than or equal to 7, include insane missions
                    {
                        thirdRandom = Random.Range(0f, 3f);
                    }
                    else if (hubTracker.allBots[0].level >= 5 || hubTracker.allBots[1].level >= 5 || hubTracker.allBots[2].level >= 5 || hubTracker.allBots[3].level >= 5) //if any bots level is greater than or equal to 5, include hard missions
                    {
                        thirdRandom = Random.Range(0f, 2f);
                    }
                    else //if the bots levels are too low to satify any of the above requirements, only include easy and normal
                    {
                        thirdRandom = Random.Range(0f, 1f);
                    }

                    difficulty.text = difficultySetting[Mathf.RoundToInt(thirdRandom)];

                    if(Mathf.RoundToInt(thirdRandom) == 0f) //easy
                    {
                        rewardAmt = 20;
                        rewardAmt1 = 10;
                        float fifthRandom = Random.Range(-10, 10);
                        float sixthRandom = Random.Range(-10, 10);
                        rewardAmt += Mathf.RoundToInt(fifthRandom);
                        rewardAmt1 += Mathf.RoundToInt(sixthRandom);
                        rewards1.text = rewardAmt.ToString();
                        rewards2.text = rewardAmt1.ToString();
                    }

                    if (Mathf.RoundToInt(thirdRandom) == 1f) //normal
                    {
                        rewardAmt = 30;
                        rewardAmt1 = 15;
                        float fifthRandom = Random.Range(-10, 10);
                        float sixthRandom = Random.Range(-10, 10);
                        rewardAmt += Mathf.RoundToInt(fifthRandom);
                        rewardAmt1 += Mathf.RoundToInt(sixthRandom);
                        rewards1.text = rewardAmt.ToString();
                        rewards2.text = rewardAmt1.ToString();
                    }

                    if (Mathf.RoundToInt(thirdRandom) == 2f) //hard
                    {
                        rewardAmt = 40;
                        rewardAmt1 = 20;
                        float fifthRandom = Random.Range(-10, 10);
                        float sixthRandom = Random.Range(-10, 10);
                        rewardAmt += Mathf.RoundToInt(fifthRandom);
                        rewardAmt1 += Mathf.RoundToInt(sixthRandom);
                        rewards1.text = rewardAmt.ToString();
                        rewards2.text = rewardAmt1.ToString();
                    }

                    if (Mathf.RoundToInt(thirdRandom) == 3f) //insane
                    {
                        rewardAmt = 50;
                        rewardAmt1 = 25;
                        float fifthRandom = Random.Range(-10, 10);
                        float sixthRandom = Random.Range(-10, 10);
                        rewardAmt += Mathf.RoundToInt(fifthRandom);
                        rewardAmt1 += Mathf.RoundToInt(sixthRandom);
                        rewards1.text = rewardAmt.ToString();
                        rewards2.text = rewardAmt1.ToString();
                    }
                }
            }
        }
    }

    public void GenerateNewMissions()
    {
        if(currentCash >= 30)
        {
            foreach (GameObject child in GameObject.FindGameObjectsWithTag("Mission"))
            {
                Destroy(child);
            }

            generated = false;
        }
    }
}
