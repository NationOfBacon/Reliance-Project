using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class RetreatState : State
{
    private static RetreatState _instance; //only declared once

    private RetreatState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static RetreatState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new RetreatState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        Vector3 destination = owner.transform.position;

        for (int i = 0; i < owner.friendlyBots.Count; i++)
        {
            if (Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) <= owner.retreatRadius && Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) >= 50)
            {
                destination = owner.friendlyBots[i].transform.position;
                break;
            }
        }

        owner.logD.RecieveLog(owner.displayName + " is attempting to retreat!"); //generate a log stating that the bot is retreating

        while (owner.botMachine.currentState == RetreatState.instance) //called from bots in the attack state when they are outnumbered
        {
            if (!owner.disabled)
            {
                if (RotateBody(owner, destination))
                {
                    owner.thisAgent.SetDestination(destination); //set the destination for the agent to the destination variable that was generated before entering the while loop

                    Debug.DrawLine(owner.transform.position, destination, Color.green); //draw a debug line that shows the destination the bot should retreat to

                    if (Vector3.Distance(owner.transform.position, destination) <= 5) //if the distance between the bot and the destination is less than 5, change to the search state
                    {
                        owner.canSeeEnemy = false;
                        owner.enemyObject = null;
                        owner.enemyHealth = null;
                        owner.additionalEnemies.Clear(); //clear the bots list of enemies

                        if (owner.botMachine.baseState == SearchState.instance)
                            owner.botMachine.ChangeState(SearchState.instance);
                        else if (owner.botMachine.baseState == CaptureAreaState.instance)
                            owner.botMachine.ChangeState(CaptureAreaState.instance);
                        else if (owner.botMachine.baseState == PartnerState.instance)
                            owner.botMachine.ChangeState(PartnerState.instance);
                        else if (owner.botMachine.baseState == MoveToState.instance)
                            owner.botMachine.ChangeState(MoveToState.instance);
                    }
                }
            }
            else
            {
                owner.thisAgent.SetDestination(owner.transform.position);
            }
            yield return null;
        }
    }

    private bool RotateBody(AIMachine _owner, Vector3 navPoint) //called to have the bot rotate to face the waypoint before navigating to it
    {
        //declare variables that will be used to determine how to move the bot
        Quaternion targetRot = Quaternion.LookRotation(navPoint - _owner.transform.position);

        //rotate the bot over time to face the enemy
        _owner.transform.rotation = Quaternion.RotateTowards(_owner.transform.rotation, targetRot, Time.deltaTime * _owner.bodyRotateSpeed);

        //set the bots y euler angle so that it only moves on that transform
        _owner.transform.localEulerAngles = new Vector3(0, _owner.transform.localEulerAngles.y, 0);


        if (Vector3.Angle(_owner.transform.forward, navPoint - _owner.transform.position) < 10)
            return true;
        else
            return false;
    }

}
