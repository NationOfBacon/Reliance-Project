using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;
using UnityEngine.AI;

public class EnemyRetreatState : EnemyState
{
    private static EnemyRetreatState _instance; //only declared once

    private EnemyRetreatState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyRetreatState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyRetreatState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner) //because of the checks done before coming to this state, there will always be a bot in range to retreat to
    {
        Vector3 destination = owner.transform.position;

        for (int i = 0; i < owner.movingBotsOnly.Count; i++) //for all moving bots, check if any of them are close enough to retreat to
        {
            if (Vector3.Distance(owner.movingBotsOnly[i].transform.position, owner.transform.position) <= owner.retreatRadius && Vector3.Distance(owner.movingBotsOnly[i].transform.position, owner.transform.position) >= 50)
            {
                destination = owner.movingBotsOnly[i].transform.position;
                break;
            }
        }

        owner.logD.RecieveLog(owner.displayName + " is attempting to retreat!"); //generate a log stating that the bot is retreating

        while (owner.botMachine.currentState == EnemyRetreatState.instance) //called from bots in the attack state when they are outnumbered
        {
            if(!owner.disabled)
            {
                if (RotateBody(owner, destination))
                {
                    owner.thisAgent.SetDestination(destination); //set the destination for the agent to the destination variable that was generated before entering the while loop

                    Debug.DrawLine(owner.transform.position, destination, Color.green); //draw a debug line that shows the destination the bot should retreat to

                    if (Vector3.Distance(owner.transform.position, destination) <= 5) //if the distance between the bot and the destination is less than 5, change to the search state
                    {
                        owner.canSeeEnemy = false;
                        owner.enemyObject = null;
                        owner.health = null;
                        owner.additionalEnemies.Clear(); //clear the bots list of enemies

                        if (owner.insideNormalSearch)
                            owner.botMachine.ChangeState(EnemySearchState.instance);
                        else if (owner.insideDefend)
                            owner.botMachine.ChangeState(EnemyMoveToState.instance);
                        else if (owner.insidePartner)
                            owner.botMachine.ChangeState(EnemyPartnerState.instance);
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

    private bool RotateBody(EnemyAIMachine _owner, Vector3 navPoint) //called to have the bot rotate to face the waypoint before navigating to it
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
