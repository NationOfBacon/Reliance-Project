using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllyAIMachineTools;

public class AllyRetreatState : AllyState
{
    private static AllyRetreatState _instance; //only declared once

    private AllyRetreatState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AllyRetreatState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new AllyRetreatState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIAllyMachine owner)
    {
        while (owner.botMachine.currentState == AllyRetreatState.instance)
        {
            if(!owner.disabled)
            {
                if (RotateBody(owner, owner.regroupPos))
                {
                    owner.thisAgent.SetDestination(owner.regroupPos);

                    Debug.DrawLine(owner.transform.position, owner.regroupPos, Color.green);

                    if (Vector3.Distance(owner.transform.position, owner.regroupPos) <= owner.stopDistance)
                    {
                        owner.canSeeEnemy = false;
                        owner.enemyObject = null;
                        owner.enemyHealth = null;
                        owner.additionalEnemies.Clear();
                        owner.botMachine.ChangeState(AllyIdleState.instance);
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

    private bool RotateBody(AIAllyMachine _owner, Vector3 navPoint) //called to have the bot rotate to face the waypoint before navigating to it
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