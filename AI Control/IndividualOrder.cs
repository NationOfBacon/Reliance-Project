using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndividualOrder : MonoBehaviour
{
    private GameObject greenUnit;
    private GameObject blueUnit;
    private GameObject orangeUnit;
    private AIMachine aiControlGreen;
    private AIMachine aiControlBlue;
    private AIMachine aiControlOrange;
    private List<GameObject> cmdButtons = new List<GameObject>();
    private GameObject commSubPanel;

    private bool blueSelected;
    private bool greenSelected;
    private bool orangeSelected;

    public bool leadTargeted;
    public bool blueTargeted;
    public bool greenTargeted;
    public bool orangeTargeted;

    private HUBTracker hubTracker;

    private void Awake()
    {
        greenUnit = GameObject.Find("AISphere Green");
        blueUnit = GameObject.Find("AISphere Blue");
        orangeUnit = GameObject.Find("AISphere Orange");
        hubTracker = GameObject.Find("Persistent Object").GetComponent<HUBTracker>();
        commSubPanel = GameObject.Find("RunningUI/Commands Group/Command Sub-Panel");
        aiControlGreen = greenUnit.GetComponent<AIMachine>();
        aiControlBlue = blueUnit.GetComponent<AIMachine>();
        aiControlOrange = orangeUnit.GetComponent<AIMachine>();

        foreach(Transform button in commSubPanel.GetComponentsInChildren<Transform>())
        {
            cmdButtons.Add(button.gameObject);
        }

        blueSelected = false;
        greenSelected = false;
        orangeSelected = false;

        leadTargeted = false;
        blueTargeted = false;
        greenTargeted = false;
        orangeTargeted = false;

        for (int i = 0; i < cmdButtons.Count; i++) //use this for loop to determine what buttons should be deactivated before the level starts
        {
            if (cmdButtons[i].name.Contains("Heal"))
            {
                cmdButtons[i].SetActive(false);
            }
        }
    }

    public void BlueSelect()
    {
        blueSelected = true;
        greenSelected = false;
        orangeSelected = false;

        if (hubTracker.blueBot.spec == "No Specialization") //if the bot has no spec, make sure that all special buttons are turned off
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.blueBot.spec == "Medic") //activate all buttons related to that bots role and deactivate any that do not
        {
            for(int i = 0; i < cmdButtons.Count; i++)
            {
                if(cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(true);
                }
            }
        }

        if (hubTracker.blueBot.spec == "Support") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.blueBot.spec == "Heavy") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.blueBot.spec == "Sniper") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }
    }

    public void GreenSelect()
    {
        blueSelected = false;
        greenSelected = true;
        orangeSelected = false;

        if (hubTracker.greenBot.spec == "No Specialization") //if the bot has no spec, make sure that all special buttons are turned off
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.greenBot.spec == "Medic") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(true);
                }
            }
        }

        if (hubTracker.greenBot.spec == "Support") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.greenBot.spec == "Heavy") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.greenBot.spec == "Sniper") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }
    }

    public void OrangeSelect()
    {
        blueSelected = false;
        greenSelected = false;
        orangeSelected = true;

        if (hubTracker.orangeBot.spec == "No Specialization") //if the bot has no spec, make sure that all special buttons are turned off
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.orangeBot.spec == "Medic") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(true);
                }
            }
        }

        if (hubTracker.orangeBot.spec == "Support") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.orangeBot.spec == "Heavy") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }

        if (hubTracker.orangeBot.spec == "Sniper") //activate all buttons related to that bots role and deactivate any that do not
        {
            for (int i = 0; i < cmdButtons.Count; i++)
            {
                if (cmdButtons[i].name.Contains("Heal"))
                {
                    cmdButtons[i].SetActive(false);
                }
            }
        }
    }

    public void Search()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(SearchState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(SearchState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(SearchState.instance);
    }

    public void Idle()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(IdleState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(IdleState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(IdleState.instance);
    }

    public void Follow()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(FollowState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(FollowState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(FollowState.instance);
    }

    public void Sentry()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(SentryState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(SentryState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(SentryState.instance);
    }

    public void MoveTo()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(MoveToState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(MoveToState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(MoveToState.instance);
    }

    public void Capture()
    {
        if (blueSelected)
        {
            if (aiControlBlue.objectives.Count == 0)
                aiControlBlue.logD.RecieveLog("No objective can be located, " + aiControlBlue.displayName + " cannot enter the capture state");
            else
                aiControlBlue.botMachine.ChangeState(CaptureAreaState.instance);
        }

        if (greenSelected)
        {
            if (aiControlGreen.objectives.Count == 0)
                aiControlGreen.logD.RecieveLog("No objective can be located, " + aiControlGreen.displayName + " cannot enter the capture state");
            else
                aiControlGreen.botMachine.ChangeState(CaptureAreaState.instance);
        }

        if (orangeSelected)
        {
            if (aiControlOrange.objectives.Count == 0)
                aiControlOrange.logD.RecieveLog("No objective can be located, " + aiControlOrange.displayName + " cannot enter the capture state");
            else
                aiControlOrange.botMachine.ChangeState(CaptureAreaState.instance);
        }
    }

    public void Partner()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(PartnerState.instance);

        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(PartnerState.instance);

        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(PartnerState.instance);
    }

    public void SetTarget(TextMeshProUGUI buttonText) //for use with the partner state
    {
        if (buttonText.text == "Lead" && blueSelected)
            aiControlBlue.followTarget = aiControlBlue.playerObject;
        if (buttonText.text == "Lead" && greenSelected)
            aiControlGreen.followTarget = aiControlGreen.playerObject;
        if (buttonText.text == "Lead" && orangeSelected)
            aiControlOrange.followTarget = aiControlOrange.playerObject;

        if (buttonText.text == "Blue" && blueSelected)
            print("A bot cannot follow itself");
        if (buttonText.text == "Blue" && greenSelected)
            aiControlGreen.followTarget = aiControlBlue.gameObject;
        if (buttonText.text == "Blue" && orangeSelected)
            aiControlOrange.followTarget = aiControlBlue.gameObject;

        if (buttonText.text == "Green" && greenSelected)
            print("A bot cannot follow itself");
        if (buttonText.text == "Green" && blueSelected)
            aiControlBlue.followTarget = aiControlGreen.gameObject;
        if (buttonText.text == "Green" && orangeSelected)
            aiControlOrange.followTarget = aiControlGreen.gameObject;

        if (buttonText.text == "Orange" && blueSelected)
            aiControlBlue.followTarget = aiControlOrange.gameObject;
        if (buttonText.text == "Orange" && greenSelected)
            aiControlGreen.followTarget = aiControlOrange.gameObject;
        if (buttonText.text == "Orange" && orangeSelected)
            print("A bot cannot follow itself");
    }

    public void Retreat()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(RetreatState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(RetreatState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(RetreatState.instance);
    }

    public void Destroy() //state not created yet
    {

    }

    public void Heal()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(HealState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(HealState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(HealState.instance);
    }

    public void Support()
    {
        if (blueSelected)
            aiControlBlue.botMachine.ChangeState(SupportState.instance);
        if (greenSelected)
            aiControlGreen.botMachine.ChangeState(SupportState.instance);
        if (orangeSelected)
            aiControlOrange.botMachine.ChangeState(SupportState.instance);
    }
}
