using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;

public class EnemyMoveToState : EnemyState
{
    private static EnemyMoveToState _instance; //only declared once

    private EnemyMoveToState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyMoveToState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyMoveToState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner)
    {
        while (owner.botMachine.currentState == EnemyMoveToState.instance)
        {
            if(!owner.disabled)
            {
                if (RotateBody(owner, owner.objective.transform.position))
                {
                    owner.thisAgent.SetDestination(owner.objective.transform.position);
                    Debug.DrawLine(owner.transform.position, owner.objective.transform.position);

                    if (Vector3.Distance(owner.transform.position, owner.objective.transform.position) <= owner.followDistance)
                    {
                        //go to the defend state
                        Debug.Log("Objective in range, going to defend state");
                        owner.botMachine.ChangeState(EnemyDefendState.instance);
                        yield break;
                    }
                }

                if (owner.canSeeEnemy && owner.enemyObject != null) //if an enemy is found, go to the chase state
                {
                    owner.botMachine.ChangeState(EnemyChaseState.instance);
                    yield break;
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
