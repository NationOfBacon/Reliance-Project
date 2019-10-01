using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private string difficulty;
    public int reinforcementCap;
    public int easyBotMin, easyBotMax, easyTurretMin, easyTurretMax;
    public int normalBotMin, normalBotMax, normalTurretMin, normalTurretMax;
    public int hardBotMin, hardBotMax, hardTurretMin, hardTurretMax;
    public int insaneBotMin, insaneBotMax, insaneTurretMin, insaneTurretMax;
    public int spawnDistance;
    private GameManager gameManager;
    private SpawnObjectives spawnObjs;
    private DisplayLog logD;
    public GameObject movingBot;
    public GameObject turretBot;
    private List<GameObject> movingBotSpawns = new List<GameObject>();
    private List<GameObject> turretBotSpawns = new List<GameObject>();
    private List<GameObject> reinforceSpawns = new List<GameObject>();
    private List<GameObject> playerBots = new List<GameObject>();


    private int reinforcementsSpawned = 0; //used to track how many enemies have been spawned as reinforcements to limit how many will show up

    public bool easy;
    public bool normal;
    public bool hard;
    public bool insane;

    private bool capReached = false;

    private void Awake()
    {
        gameManager = GameObject.Find("Persistent Object").GetComponent<GameManager>();//get reference to game manager to set the difficulty
        spawnObjs = GameObject.Find("EventSystem").GetComponent<SpawnObjectives>();
        logD = GameObject.Find("RunningUI/Text Log/Log Panel/Content").GetComponent<DisplayLog>();

        difficulty = gameManager.difficulty;

        if (difficulty == "Easy") //set the bool for what difficulty the mission is
        {
            easy = true;
            normal = false;
            hard = false;
            insane = false;
        }

        if (difficulty == "Normal")
        {
            easy = false;
            normal = true;
            hard = false;
            insane = false;
        }

        if (difficulty == "hard")
        {
            easy = false;
            normal = false;
            hard = true;
            insane = false;
        }

        if (difficulty == "Insane")
        {
            easy = false;
            normal = false;
            hard = false;
            insane = true;
        }


        foreach (GameObject movingBot in GameObject.FindGameObjectsWithTag("moveSpawn"))//add all moving bot spawns in the level to the list
        {
            movingBotSpawns.Add(movingBot);
        }
        foreach (GameObject turretBot in GameObject.FindGameObjectsWithTag("turretSpawn"))//add all turret spawns in the level to the list
        {
            turretBotSpawns.Add(turretBot);
        }
        foreach (GameObject reinforceSpawn in GameObject.FindGameObjectsWithTag("ReinforceSpawn"))//add all turret spawns in the level to the list
        {
            reinforceSpawns.Add(reinforceSpawn);
        }
        foreach (GameObject playerBot in GameObject.FindGameObjectsWithTag("Target"))//add the player and friendly bots to a list
        {
            if(playerBot.GetComponent<AIMachine>() || playerBot.GetComponent<PlayerShoot>() || playerBot.GetComponent<AIAllyMachine>())
                playerBots.Add(playerBot);
        }



        if (difficulty == "Easy") //for each difficulty, spawn in the enemies based on the range of the random numbers
        {
            int randInt = Random.Range(easyBotMin, easyBotMax);
            int randInt1 = Random.Range(easyTurretMin, easyTurretMax);
            List<GameObject> usedSpawns = new List<GameObject>();
            
            for (int i = 0; i < randInt; i++) //for bots
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, movingBotSpawns.Count);
                for(int x = 0; x < usedSpawns.Count; x++)
                {
                    if (movingBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if(sameSpawn == false)
                {
                    Instantiate(movingBot, movingBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(movingBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }

            usedSpawns.Clear(); //clear list of used spawns between enemy types

            for (int i = 0; i < randInt1; i++) //for turrets
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, turretBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (turretBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(turretBot, turretBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(turretBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }
        }
        if (difficulty == "Normal")
        {
            int randInt = Random.Range(normalBotMin, normalBotMax);
            int randInt1 = Random.Range(normalTurretMin, normalTurretMax);
            List<GameObject> usedSpawns = new List<GameObject>();

            for (int i = 0; i < randInt; i++) //for bots
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, movingBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (movingBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(movingBot, movingBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(movingBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }

            usedSpawns.Clear(); //clear list of used spawns between enemy types

            for (int i = 0; i < randInt1; i++) //for turrets
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, turretBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (turretBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(turretBot, turretBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(turretBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }
        }
        if (difficulty == "Hard")
        {
            int randInt = Random.Range(hardBotMin, hardBotMax);
            int randInt1 = Random.Range(hardTurretMin, hardTurretMax);
            List<GameObject> usedSpawns = new List<GameObject>();

            for (int i = 0; i < randInt; i++) //for bots
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, movingBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (movingBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(movingBot, movingBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(movingBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }

            usedSpawns.Clear(); //clear list of used spawns between enemy types

            for (int i = 0; i < randInt1; i++) //for turrets
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, turretBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (turretBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(turretBot, turretBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(turretBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }
        }
        if (difficulty == "Insane")
        {
            int randInt = Random.Range(insaneBotMin, insaneBotMax);
            int randInt1 = Random.Range(insaneTurretMin, insaneTurretMax);
            List<GameObject> usedSpawns = new List<GameObject>();

            for (int i = 0; i < randInt; i++) //for bots
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, movingBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (movingBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(movingBot, movingBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(movingBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }

            usedSpawns.Clear(); //clear list of used spawns between enemy types

            for (int i = 0; i < randInt1; i++) //for turrets
            {
                bool sameSpawn = false;
                int randomSpawn = Random.Range(0, turretBotSpawns.Count);
                for (int x = 0; x < usedSpawns.Count; x++)
                {
                    if (turretBotSpawns[randomSpawn] == usedSpawns[x])
                    {
                        sameSpawn = true;
                        break;
                    }
                }

                if (sameSpawn == false)
                {
                    Instantiate(turretBot, turretBotSpawns[randomSpawn].transform);
                    usedSpawns.Add(turretBotSpawns[randomSpawn]);
                }
                else
                    i--;
            }
        }

        Debug.Log("Enemy Spawning Complete");
    }

    private void Start()
    {
        gameManager.BotSetup();
    }

    private void Update()
    {
        if(gameManager.missionName == "Assault")
        {
            if (gameManager.remainingEnemies <= 3 && reinforcementsSpawned < reinforcementCap && spawnObjs.capAreaInstance.GetComponent<CaptureArea>().roundedRate > 0)
            {
                Debug.Log("<color=yellow>Spawning reinforcements</color>");
                SpawnReinforcements();
            }

            if (reinforcementsSpawned >= reinforcementCap && capReached == false)
            {
                Debug.Log("<color=red>Reinforcement cap reached</color>");
                capReached = true;
            }
        }
        else if (gameManager.missionName == "Hack")
        {
            if (gameManager.remainingEnemies <= 3 && reinforcementsSpawned < reinforcementCap && spawnObjs.terminalInstance.GetComponent<CaptureArea>().roundedRate > 0)
            {
                Debug.Log("<color=yellow>Spawning reinforcements</color>");
                SpawnReinforcements();
            }

            if (reinforcementsSpawned >= reinforcementCap && capReached == false)
            {
                Debug.Log("<color=red>Reinforcement cap reached</color>");
                capReached = true;
            }
        }
        else if (gameManager.missionName == "Defend")
        {
            Health structureHP = spawnObjs.structureInstance.GetComponent<Health>();
            if (gameManager.remainingEnemies <= 3 && reinforcementsSpawned < reinforcementCap && structureHP.health < structureHP.maxHealth && spawnObjs.structureInstance != null)
            {
                Debug.Log("<color=yellow>Spawning reinforcements</color>");
                SpawnReinforcements();
            }

            if (reinforcementsSpawned >= reinforcementCap && capReached == false)
            {
                Debug.Log("<color=red>Reinforcement cap reached</color>");
                capReached = true;
            }
        }
        else if (gameManager.missionName == "Destroy")
        {
            Health structureHP = spawnObjs.structureInstance.GetComponent<Health>();
            if (gameManager.remainingEnemies <= 3 && reinforcementsSpawned < reinforcementCap && structureHP.health < structureHP.maxHealth && spawnObjs.structureInstance != null)
            {
                Debug.Log("<color=yellow>Spawning reinforcements</color>");
                SpawnReinforcements();
            }

            if (reinforcementsSpawned >= reinforcementCap && capReached == false)
            {
                Debug.Log("<color=red>Reinforcement cap reached</color>");
                capReached = true;
            }
        }
        else if (gameManager.missionName == "Protect")
        {
            if(gameManager.allyBots.Count >= 1 && gameManager.remainingEnemies <= 3 && reinforcementsSpawned <= reinforcementCap)
            {
                Debug.Log("<color=yellow>Spawning reinforcements</color>");
                SpawnReinforcements();
            }

            if (reinforcementsSpawned >= reinforcementCap && capReached == false)
            {
                Debug.Log("<color=red>Reinforcement cap reached</color>");
                capReached = true;
            }
        }
    }

    public void SpawnReinforcements() //called when the mission type needs more enemies to spawn in
    {
        int unitAmt = Random.Range(1, reinforcementCap);
        int enemiesSpawned = 0;

        logD.RecieveLog("Enemy reinforcements have appeared!!");

        for (int i = 0; i < reinforceSpawns.Count; i++) //for each spawn
        {
            float minDistance = float.MaxValue;

            for (int y = 0; y < playerBots.Count; y++) //check EACH bots distance to the current spawn and then decide to spawn or not
            {
                var botDist = Vector3.Distance(reinforceSpawns[i].transform.position, playerBots[y].transform.position);

                if (botDist < minDistance) //get the shortest distance to this spawn
                {
                    minDistance = botDist;
                }
            }

            //if the current spawn point is at a good range and the reinforcement cap has not been reached yet, spawn an enemy
            if (minDistance >= spawnDistance && enemiesSpawned < unitAmt && reinforcementsSpawned <= reinforcementCap)
            {
                var newEnemy = Instantiate(movingBot, reinforceSpawns[i].transform);
                gameManager.movingEnemies.Add(newEnemy);
                newEnemy.GetComponent<EnemyAIMachine>().defender = true;
                gameManager.remainingEnemies++;
                reinforcementsSpawned++;
                enemiesSpawned++;
                Debug.Log("Extra enemies spawned: " + reinforcementsSpawned);
            }

        }
    }
}
