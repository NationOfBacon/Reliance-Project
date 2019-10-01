using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject runningUI;
    private GameObject finishUI;
    private GameObject escapeUI;
    private GameObject startUI;
    private GameObject deathUI;
    private GameObject commandMenu;
    private GameObject subCommMenu;
    private GameObject subCommMenu1;
    private GameObject subCommMenu2;
    private GameObject loadoutUI;
    private GameObject inventoryUI;
    private GameObject missionUI;
    private GameObject informationUI;
    private GameManager gameManager;
    private SoundManager soundManager;
    private GameObject escapeText;
    private GameObject escapeBars;
    public bool objectsSet;
    public bool hubObjectsSet;

    public bool escapeMenuOpen;
    public bool commandMenuOpen;

    private Cinemachine.CinemachineBrain virtCam;


    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        soundManager = GetComponent<SoundManager>();
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "HUB scene" && scene.name != "Main Menu")
        {
            if(!startUI.activeSelf)
            {
                if (!escapeMenuOpen && Input.GetButtonDown("Escape"))
                {
                    if (!escapeUI.activeSelf)
                    {
                        escapeUI.SetActive(true);
                        gameManager.PauseBots();
                        gameManager.PausePlayer();
                        escapeMenuOpen = true;
                    }
                }
                else if (escapeMenuOpen && Input.GetButtonDown("Escape"))
                {
                    if (escapeUI.activeSelf)
                    {
                        escapeUI.SetActive(false);
                        gameManager.UnPauseBots();
                        gameManager.UnPausePlayer();
                        escapeMenuOpen = false;
                    }
                }

                if (!commandMenuOpen && Input.GetButtonDown("Open AI Command Menu"))
                {
                    if (!commandMenu.activeSelf)
                    {
                        commandMenu.SetActive(true);
                        commandMenuOpen = true;
                    }
                }
                else if (commandMenuOpen && Input.GetButtonDown("Open AI Command Menu"))
                {
                    if (commandMenu.activeSelf)
                    {
                        commandMenu.SetActive(false);
                        commandMenuOpen = false;
                    }
                }

                if ((subCommMenu1.activeSelf || subCommMenu2.activeSelf) && Input.GetButtonDown("Open AI Command Menu"))
                {
                    subCommMenu.SetActive(true);
                    subCommMenu1.SetActive(false);
                    subCommMenu2.SetActive(false);
                }
            }
            else
            {
                gameManager.PauseBots();

                if (Input.anyKeyDown)
                {
                    startUI.SetActive(false);
                    gameManager.UnPauseBots();
                    gameManager.OnMissionStart();
                }
            }
        }
    }

    public void OnLevelLoad(Scene scene)
    {
        if (scene.name != "HUB scene" && scene.name != "Main Menu")
        {
            runningUI = GameObject.Find("RunningUI");
            finishUI = GameObject.Find("FinishUI");
            escapeUI = GameObject.Find("EscapeUI");
            startUI = GameObject.Find("StartUI");
            deathUI = GameObject.Find("DeathUI");
            escapeText = GameObject.Find("RunningUI/Escape Text");
            escapeBars = GameObject.Find("RunningUI/Escape Bars");
            commandMenu = GameObject.Find("RunningUI/Commands Group");
            subCommMenu = GameObject.Find("RunningUI/Commands Group/Command Panel");
            subCommMenu1 = GameObject.Find("RunningUI/Commands Group/Command Sub-Panel");
            subCommMenu2 = GameObject.Find("RunningUI/Commands Group/Command Context Panel");
            virtCam = Camera.main.gameObject.GetComponent<Cinemachine.CinemachineBrain>();
            RunningUISet();
            escapeMenuOpen = false;
        }

        if (scene.name == "HUB scene")
        {
            missionUI = GameObject.Find("HUBUI/Missions Panel");
            loadoutUI = GameObject.Find("HUBUI/Loadout Panel");
            inventoryUI = GameObject.Find("HUBUI/Inventory Panel");
            informationUI = GameObject.Find("HUBUI/Information Panel");
        }
    }

    public void EscapeTrigger()
    {
        escapeText.SetActive(true);
        escapeBars.SetActive(true);
    }

    public void FinishScreen()
    {
        runningUI.SetActive(false);
        finishUI.SetActive(true);
        gameManager.PauseBots();
        gameManager.PausePlayer();
    }

    //private IEnumerator StartSequence()
    //{
    //    var text = startUI.GetComponentInChildren<TextMeshProUGUI>();
    //    text.text = "5";
    //    yield return new WaitForSecondsRealtime(1);
    //    text.text = "4";
    //    yield return new WaitForSecondsRealtime(1);
    //    text.text = "3";
    //    yield return new WaitForSecondsRealtime(1);
    //    text.text = "2";
    //    yield return new WaitForSecondsRealtime(1);
    //    text.text = "1";
    //    yield return new WaitForSecondsRealtime(1);
    //    Time.timeScale = 1f;
    //    startUI.SetActive(false);
    //    virtCam.enabled = true;
    //}

    public void PlayerDeath() //called when player dies or the objective is failed
    {
        deathUI.SetActive(true);
        gameManager.PauseBots();
        if (gameManager.playerObject.activeSelf) //if the player is not dead, pause them
            gameManager.PausePlayer();
    }

    public void HUBUISet()
    {
        loadoutUI.SetActive(false);
        inventoryUI.SetActive(false);
        informationUI.SetActive(false);
    }

    public void RunningUISet()
    {
        escapeText.SetActive(false);
        escapeBars.SetActive(false);
        commandMenu.SetActive(false);
        finishUI.SetActive(false);
        escapeUI.SetActive(false);
        deathUI.SetActive(false);
    }
}
