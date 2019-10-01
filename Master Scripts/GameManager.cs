using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject playerObject;
    public List<GameObject> allEnemies;
    public int startingEnemyCount;
    public List<GameObject> movingEnemies = new List<GameObject>();
    public List<GameObject> friendlyBots = new List<GameObject>();
    public List<GameObject> allyBots = new List<GameObject>();
    public int remainingEnemies;
    public List<GameObject> spawns;
    public bool exitOpen = false;
    public bool missionComplete = false;
    public string difficulty;
    public string missionName;
    private GameObject capArea;
    private GameObject terminal;
    private GameObject structure;

    private UIManager uiManager;
    private SoundManager soundMgr;
    private HUBTracker hubTracker;
    private SpawnObjectives spawnObjs;
    private InventoryManager invMgr;
    private SaveFileManager saveMgr;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        hubTracker = GetComponent<HUBTracker>();
        soundMgr = GetComponent<SoundManager>();
        invMgr = GetComponent<InventoryManager>();
        saveMgr = GetComponent<SaveFileManager>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //when a scene is loaded, this calls to the OnSceneLoaded method
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //special method used by the scene manager to do things when a scene is loaded
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        soundMgr.SetAudio(scene); //call to the sound manager to set the audio for the scene
        hubTracker.OnLevelLoad(scene);
        uiManager.OnLevelLoad(scene);
        invMgr.OnLevelLoad(scene);
        saveMgr.OnLevelLoad(scene);


        if (scene.name == "Level 1")
        {
            spawnObjs = GameObject.Find("EventSystem").GetComponent<SpawnObjectives>();
            playerObject = GameObject.Find("PlayerSphere");
            PausePlayer();

            foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Target"))
            {
                if (bot.GetComponent<AIMachine>())
                    friendlyBots.Add(bot);
            }

            if (missionName == "Defend")
            {
                structure = spawnObjs.structureInstance;
            }
            else if (missionName == "Protect")
            {
                foreach (GameObject ally in GameObject.FindGameObjectsWithTag("Target"))
                {
                    if (ally.GetComponent<AIAllyMachine>())
                    {
                        allyBots.Add(ally);
                    }
                }
            }
            else if (missionName == "Destroy")
            {
                structure = spawnObjs.structureInstance;
            }
            else if (missionName == "Assault")
            {
                capArea = spawnObjs.capAreaInstance;
            }
            else if (missionName == "Hack")
            {
                terminal = spawnObjs.terminalInstance;
            }
            else if (missionName == "Kill")
            {

            }
        }

        if (scene.name == "HUB scene" || scene.name == "Main Menu") //if the player is in the hub
        {
            invMgr.addCurrency = true;

            if (missionComplete == false) //if the mission was not complete when the player returned to the hub, reset the currency earned
                invMgr.addCurrency = false;

            invMgr.ModifyCurrency(); //after the above code decides what the addCurrency bool should be, call to the invMgr to change the currency

            playerObject = null;
            allEnemies.Clear();
            movingEnemies.Clear();
            allyBots.Clear();
            friendlyBots.Clear();
            remainingEnemies = 0;
            spawns = null;
            exitOpen = false;
            missionComplete = false;
        }
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "HUB scene" && scene.name != "Main Menu") //if the player is in a mission
        {
            //Based on the mission name, set the victory conditions for each type of mission

            if (missionName == "Defend")
            {
                // victory will be achieved after a set number of enemy waves have been cleared

                if (structure.activeSelf && allEnemies.Count <= 0)
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }

                if(!structure.activeSelf)
                {
                    OnMissionFail();
                }
            }
            else if (missionName == "Protect")
            {
                // victory will be achieved after all enemies are cleared and at least 1 friendly unit is alive
                for (int i = 0; i < allyBots.Count; i++)
                {
                    if (!allyBots[i].activeSelf)
                    {
                        allyBots.Remove(allyBots[i]);
                    }
                }

                if(allEnemies.Count <= 0 && allyBots.Count >= 1)
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }

                if(allyBots.Count <= 0)
                {
                    OnMissionFail();
                }
            }
            else if (missionName == "Destroy")
            {
                // victory will be achived after the enemy structure is destroyed

                if (!structure.activeSelf)
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }
            }
            else if (missionName == "Assault")
            {
                if(capArea.GetComponent<CaptureArea>().roundedRate >= 100)
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }
            }
            else if (missionName == "Hack")
            {
                if (terminal.GetComponent<CaptureArea>().roundedRate >= 100)
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }
            }
            else if (missionName == "Kill")
            {
                if (remainingEnemies <= 0) //if there are no enemies left, display the "Escape" UI
                {
                    uiManager.EscapeTrigger();
                    exitOpen = true;
                }
            }

            if (remainingEnemies < 0) //if the enemy count is negative, set it to 0
            {
                remainingEnemies = 0;
            }

            if (!playerObject.activeSelf) //if the player object is deactivated, call to PlayerDeath in the UIManager
            {
                uiManager.PlayerDeath();
            }
        }
    }

    public void PausePlayer()
    {
        playerObject.GetComponent<PlayerShoot>().enabled = false;
        playerObject.GetComponent<PlayerMove>().enabled = false;
    }

    public void UnPausePlayer()
    {
        playerObject.GetComponent<PlayerShoot>().enabled = true;
        playerObject.GetComponent<PlayerMove>().enabled = true;
    }

    public void OnKill(GameObject targetEnemy) //run this whenever an enemy is killed
    {
        allEnemies.Remove(targetEnemy);
        remainingEnemies = allEnemies.Count;
    }

    public void BotSetup() //called when spawning enemies to set lists and leader bots
    {
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (bot.GetComponent<Health>())
                allEnemies.Add(bot);
        }

        startingEnemyCount = allEnemies.Count;

        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(bot.GetComponent<Health>())
            {
                if (bot.GetComponent<EnemyAIMachine>().turretBot == false)
                    movingEnemies.Add(bot);
            }
        }

        if(movingEnemies.Count >= 9)
        {
            for (int i = 0; i < 3; i++) //sets number of leaders for use in the EnemyPartnerState
            {
                int leaderSelect = Random.Range(0, movingEnemies.Count);
                movingEnemies[leaderSelect].GetComponent<EnemyAIMachine>().leader = true;
            }
        }

        if(missionName == "Defend" || missionName == "Assault" || missionName == "Protect" || missionName == "Destroy" || missionName == "Hack")
        {
            int defenderSelect = Random.Range(2, movingEnemies.Count - 2); //select a random number of bots and set them as defenders

            for(int i = 0; i < defenderSelect; i++)
            {
                movingEnemies[i].GetComponent<EnemyAIMachine>().defender = true;
            }
        }

        for(int i = 0; i < allEnemies.Count; i++) //turn each bot off so that they do not move around before the player starts
        {
            allEnemies[i].SetActive(false);
        }

        LevelSetup();
    }

    public void LevelSetup() //called after setting the bots to establish default values for the level
    {
        remainingEnemies = allEnemies.Count;
    }

    public void OnMissionFail() //called to display the death screen when an objective is failed or the player dies
    {
        uiManager.PlayerDeath();
    }

    public void OnMissionStart() //called when the player presses a button to remove the start UI for a level
    {
        UnPausePlayer();

        for (int i = 0; i < allEnemies.Count; i++) //turn each bot on when the player starts
        {
            allEnemies[i].SetActive(true);
        }
    }

    public void PauseBots() //called to stop enemy bots functions when the game is paused
    {
        for (int i = 0; i < allEnemies.Count; i++) //turn each bot on when the player starts
        {
            allEnemies[i].GetComponent<EnemyAIMachine>().disabled = true;
        }

        for(int i = 0; i < friendlyBots.Count; i++)
        {
            friendlyBots[i].GetComponent<AIMachine>().disabled = true;
        }

        for(int i = 0; i < allyBots.Count; i++)
        {
            allyBots[i].GetComponent<AIAllyMachine>().disabled = true;
        }
    }

    public void UnPauseBots() //called to stop enemy bots functions when the game is paused
    {
        for (int i = 0; i < allEnemies.Count; i++) //turn each bot on when the player starts
        {
            allEnemies[i].GetComponent<EnemyAIMachine>().disabled = false;
        }

        for (int i = 0; i < friendlyBots.Count; i++)
        {
            friendlyBots[i].GetComponent<AIMachine>().disabled = false;
        }

        for (int i = 0; i < allyBots.Count; i++)
        {
            allyBots[i].GetComponent<AIAllyMachine>().disabled = false;
        }
    }
}
