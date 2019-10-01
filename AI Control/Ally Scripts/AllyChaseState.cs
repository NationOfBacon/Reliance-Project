using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllyAIMachineTools;

public class AllyChaseState : AllyState
{
    private static AllyChaseState _instance; //only declared once

    private AllyChaseState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AllyChaseState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new AllyChaseState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIAllyMachine owner)
    {
        while (owner.botMachine.currentState == AllyChaseState.instance)
        {
            if(!owner.disabled)
            {
                if (owner.enemyObject == null)
                {
                    owner.canSeeEnemy = false;
                    owner.enemyHealth = null;
                    owner.botMachine.ChangeState(AllyRetreatState.instance); //go to a retreat state
                    yield break;
                }

                if (owner.enemyObject != null)
                {
                    owner.thisAgent.SetDestination(owner.transform.position);

                    if (RotateBody(owner, owner.enemyObject.transform.position))
                    {
                        owner.thisAgent.SetDestination(owner.enemyObject.transform.position);

                        if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy)
                        {
                            owner.botMachine.ChangeState(AllyAttackState.instance);
                            yield break;
                        }
                    }

                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance & owner.canSeeEnemy)
                    {
                        owner.botMachine.ChangeState(AllyAttackState.instance);
                        yield break;
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