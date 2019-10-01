using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MouseTools;

public class MouseOverBot : MonoBehaviour //placed on bots with a bool manaully set to tell the cursor what icon to switch to
{
    private CustomCursor mouseController;
    public bool greenBot;
    public bool blueBot;
    public bool orangeBot;
    public bool leadBot;
    public bool enemyBot;

    private void Start()
    {
        mouseController = GameObject.Find("RunningUI/Mouse Base").GetComponent<CustomCursor>();
    }

    private void OnMouseEnter()
    {
        if(!mouseController.botSelected) //if there is not a bot selected by the cursor, change the cursor
        {
            setCursor();
        }

        if (enemyBot)
            mouseController.overEnemy = true;
        else
            mouseController.overBot = true;

    }

    //private void OnMouseOver()
    //{
    //    if(Input.GetKeyDown(KeyCode.Mouse1) && !mouseController.overEnemy)
    //    {
    //        mouseController.botSelected = true;
    //        SetBotTargetBool();
    //        SetBotAIMachine();
    //    }

    //    if(Input.GetKeyDown(KeyCode.Mouse1) && mouseController.botSelected && !mouseController.overEnemy) //if a bot is already selected but another bot is clicked, switch to that one
    //    {
    //        setCursor();
    //        SetBotTargetBool();
    //        SetBotAIMachine();
    //    }

    //    if (enemyBot)
    //        mouseController.targetEnemy = gameObject;
    //}

    private void OnMouseExit()
    {
        if(mouseController.botSelected == false)
        {
            mouseController.overBot = false;
            mouseController.ChangeCursor(5);
        }
        else
        {
            mouseController.overBot = false;
        }

        if (enemyBot)
            mouseController.overEnemy = false;
        else
            mouseController.overBot = false;
    }

    public void setCursor()
    {
        if (enemyBot)
        {
            mouseController.overBot = true;
            mouseController.ChangeCursor(4);
        }
        else if (greenBot)
        {
            mouseController.overBot = true;
            mouseController.ChangeCursor(0);
        }
        else if (blueBot)
        {
            mouseController.overBot = true;
            mouseController.ChangeCursor(1);
        }
        else if (orangeBot)
        {
            mouseController.overBot = true;
            mouseController.ChangeCursor(2);
        }
        else if (leadBot)
        {
            mouseController.overBot = true;
            mouseController.ChangeCursor(3);
        }
    }

    public void SetBotTargetBool() //sets target bool in customcursor
    {
        if (enemyBot)
        {
            mouseController.enemyBot = true;
            mouseController.greenBot = false;
            mouseController.blueBot = false;
            mouseController.orangeBot = false;
            mouseController.leadBot = false;
        }
        else if (greenBot)
        {
            mouseController.enemyBot = false;
            mouseController.greenBot = true;
            mouseController.blueBot = false;
            mouseController.orangeBot = false;
            mouseController.leadBot = false;
        }
        else if (blueBot)
        {
            mouseController.enemyBot = false;
            mouseController.greenBot = false;
            mouseController.blueBot = true;
            mouseController.orangeBot = false;
            mouseController.leadBot = false;
        }
        else if (orangeBot)
        {
            mouseController.enemyBot = false;
            mouseController.greenBot = false;
            mouseController.blueBot = false;
            mouseController.orangeBot = true;
            mouseController.leadBot = false;
        }
        else if (leadBot)
        {
            mouseController.enemyBot = false;
            mouseController.greenBot = false;
            mouseController.blueBot = false;
            mouseController.orangeBot = false;
            mouseController.leadBot = true;
        }
    }

    public void SetBotAIMachine()
    {
        if(!leadBot)
            mouseController.selectedBot = GetComponent<AIMachine>();
    }
}
