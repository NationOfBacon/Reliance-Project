using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;

public class EnemyChaseState : EnemyState
{
    private static EnemyChaseState _instance; //only declared once

    private EnemyChaseState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyChaseState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyChaseState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner)
    {
        while (owner.botMachine.currentState == EnemyChaseState.instance)
        {
            if(!owner.disabled)
            {
                if (owner.enemyObject == null) //if the enemy is gone, go back to search
                {
                    owner.health = null;
                    if (owner.insideNormalSearch)
                        owner.botMachine.ChangeState(EnemySearchState.instance);
                    else if (owner.insideDefend)
                        owner.botMachine.ChangeState(EnemyMoveToState.instance);
                    else if (owner.insidePartner)
                        owner.botMachine.ChangeState(EnemyPartnerState.instance);
                    yield break;
                }

                if (owner.enemyObject != null) //if the enemy is still around, go after it
                {
                    if (RotateBody(owner, owner.enemyObject.transform.position)) //if the bot is rotated towards the enemy, start moving
                    {
                        owner.thisAgent.SetDestination(owner.enemyObject.transform.position);

                        if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy) //if the enemy is in attack range and is visible, attack it
                        {
                            owner.botMachine.ChangeState(EnemyAttackState.instance);
                            yield break;
                        }
                    }

                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy) //if the enemy is in range and visible but the bot is not rotated, just attack it
                    {
                        owner.botMachine.ChangeState(EnemyAttackState.instance);
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
