using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectives : MonoBehaviour
{
    private GameManager gameManager;
    private string missionName;

    public GameObject capArea;
    public GameObject capAreaInstance;
    public GameObject terminal;
    public GameObject terminalInstance;
    public GameObject structure;
    public GameObject structureInstance;
    public GameObject enemyStructure;
    public GameObject friendlyBot;

    public List<GameObject> captureAreaSpawns = new List<GameObject>();
    public List<GameObject> terminalSpawns = new List<GameObject>();
    public List<GameObject> enemyStructureSpawns = new List<GameObject>();
    public List<GameObject> defendStructureSpawns = new List<GameObject>();
    public List<GameObject> allySpawns = new List<GameObject>();
    public List<GameObject> allFriendBotInstances = new List<GameObject>();


    private void Awake()
    {
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();
        missionName = gameManager.missionName;

        foreach (GameObject capArea in GameObject.FindGameObjectsWithTag("CapAreaSpawn"))//add all moving bot spawns in the level to the list
        {
            captureAreaSpawns.Add(capArea);
        }
        foreach (GameObject terminal in GameObject.FindGameObjectsWithTag("TerminalSpawn"))//add all turret spawns in the level to the list
        {
            terminalSpawns.Add(terminal);
        }
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("EnemyStructureSpawn"))//add all structure spawns in the level to the list
        {
            enemyStructureSpawns.Add(structure);
        }
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("DefendStructureSpawn"))//add all structure spawns in the level to the list
        {
            defendStructureSpawns.Add(structure);
        }
        foreach (GameObject ally in GameObject.FindGameObjectsWithTag("AllySpawn"))//add all ally spawns in the level to the list
        {
            allySpawns.Add(ally);
        }


        if (missionName == "Defend")
        {
            //spawn a destroyable structure that must be kept alive
            int spawnSelect = Random.Range(0, defendStructureSpawns.Count);

            structureInstance = Instantiate(structure, defendStructureSpawns[spawnSelect].transform);
        }
        else if (missionName == "Protect")
        {
            //spawn friendly bots that the player must keep alive
            int spawnAmt = Random.Range(3, allySpawns.Count);

            for(int i = 0; i < spawnAmt; i++)
            {
                allFriendBotInstances.Add(Instantiate(friendlyBot, allySpawns[i].transform));
            }
        }
        else if (missionName == "Destroy")
        {
            //create an objective object that must be destroyed
            int spawnSelect = Random.Range(0, enemyStructureSpawns.Count);

            structureInstance = Instantiate(enemyStructure, enemyStructureSpawns[spawnSelect].transform);
        }
        else if (missionName == "Assault")
        {
            Debug.Log("Mission was assault, spawning cap area");
            //spawn an area that must be captured
            int spawnSelect = Random.Range(0, captureAreaSpawns.Count);

            capAreaInstance = Instantiate(capArea, captureAreaSpawns[spawnSelect].transform);
        }
        else if (missionName == "Hack")
        {
            Debug.Log("Mission was hack, spawning terminal");
            //spawn a terminal that must be hacked
            int spawnSelect = Random.Range(0, terminalSpawns.Count);

            terminalInstance = Instantiate(terminal, terminalSpawns[spawnSelect].transform);
        }

        Debug.Log("Objective Spawning complete");
    }
}
