using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetObjectiveText : MonoBehaviour
{
    public GameObject objTextPrefab;
    private GameManager gameManager;
    private GameObject capAreaInstance;
    private GameObject terminalInstance;
    private GameObject structureInstance;
    private List<GameObject> allies = new List<GameObject>();


    private float captureAmt = 0;
    private float structureDamage = 0;

    private string missionName;
    private GameObject currentObj;

    private SpawnObjectives spawnObjs;

    private void Awake()
    {
        spawnObjs = GameObject.Find("EventSystem").GetComponent<SpawnObjectives>();
    }

    private void Start()
    {
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        missionName = gameManager.missionName;

        if (missionName == "Defend")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Defend the structure: " + structureDamage + "HP";
            structureDamage = spawnObjs.structureInstance.GetComponent<Health>().health;
            structureInstance = spawnObjs.structureInstance;
        }
        else if (missionName == "Protect")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Protect the friendly units: " + allies.Count;

            foreach (GameObject ally in GameObject.FindGameObjectsWithTag("Target"))
            {
                if(ally.GetComponent<AIAllyMachine>())
                {
                    allies.Add(ally);
                }
            }
        }
        else if (missionName == "Destroy")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Destroy the enemy structure: " + structureDamage + "HP";
            structureDamage = spawnObjs.structureInstance.GetComponent<Health>().health;
            structureInstance = spawnObjs.structureInstance;
        }
        else if (missionName == "Assault")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Capture the zone: " + captureAmt;
            capAreaInstance = spawnObjs.capAreaInstance;
        }
        else if (missionName == "Hack")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Hack into the terminal: " + captureAmt;
            terminalInstance = spawnObjs.terminalInstance;
        }
        else if (missionName == "Kill")
        {
            currentObj = Instantiate(objTextPrefab, transform);
            currentObj.GetComponent<TextMeshProUGUI>().text = "Eliminate all enemies: " + gameManager.remainingEnemies + "/" + gameManager.startingEnemyCount;
        }
    }

    private void Update()
    {
        if (missionName == "Defend")
        {
            currentObj.GetComponent<TextMeshProUGUI>().text = "Defend the structure: " + structureDamage + "HP";
            if (!structureInstance.activeSelf) //if the structure is not active, meaning it was destroyed, set damage to 0
            {
                structureDamage = 0;
            }
            else //otherwise update the damage
            {
                structureDamage = structureInstance.GetComponent<Health>().health;
            }
        }
        else if (missionName == "Protect")
        {
            for(int i = 0; i < allies.Count; i++) //for each allied bot, if any of them are inactive, remove them from the list
            {
                if(!allies[i].activeSelf)
                {
                    allies.Remove(allies[i]);
                }
            }

            if (allies.Count <= 2) //if the number of allies remaining is 2 or less, change the text to red
            {
                currentObj.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            currentObj.GetComponent<TextMeshProUGUI>().text = "Protect the friendly units: " + allies.Count;
        }
        else if (missionName == "Destroy")
        {
            currentObj.GetComponent<TextMeshProUGUI>().text = "Destroy the enemy structure: " + structureDamage + "HP";

            if (!structureInstance.activeSelf) //if the structure is not active, meaning it was destroyed, set damage to 0
            {
                structureDamage = 0;
            }
            else //otherwise update the damage
            {
                structureDamage = structureInstance.GetComponent<Health>().health;
            }
        }
        else if (missionName == "Assault")
        {
            captureAmt = capAreaInstance.GetComponent<CaptureArea>().roundedRate;
            currentObj.GetComponent<TextMeshProUGUI>().text = "Capture the zone: " + captureAmt;
        }
        else if (missionName == "Hack")
        {
            captureAmt = terminalInstance.GetComponent<CaptureArea>().roundedRate;
            currentObj.GetComponent<TextMeshProUGUI>().text = "Hack into the terminal: " + captureAmt;
        }
        else if(missionName == "Kill")
        {
            currentObj.GetComponent<TextMeshProUGUI>().text = "Eliminate all enemies: " + gameManager.remainingEnemies + "/" + gameManager.startingEnemyCount;
        }
    }
}
