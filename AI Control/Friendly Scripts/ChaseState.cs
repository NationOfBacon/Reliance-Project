using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class ChaseState : State
{
    private static ChaseState _instance; //only declared once

    private ChaseState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static ChaseState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new ChaseState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        while (owner.botMachine.currentState == ChaseState.instance)
        {
            if (!owner.disabled)
            {
                if (owner.enemyObject == null)
                {
                    owner.canSeeEnemy = false;
                    owner.enemyHealth = null;
                    if (owner.botMachine.baseState == SearchState.instance)
                        owner.botMachine.ChangeState(SearchState.instance);
                    else if (owner.botMachine.baseState == CaptureAreaState.instance)
                        owner.botMachine.ChangeState(CaptureAreaState.instance);
                    else if (owner.botMachine.baseState == PartnerState.instance)
                        owner.botMachine.ChangeState(PartnerState.instance);
                    else if (owner.botMachine.baseState == MoveToState.instance)
                        owner.botMachine.ChangeState(MoveToState.instance);
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
                            owner.botMachine.ChangeState(AttackState.instance);
                            yield break;
                        }
                    }

                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy)
                    {
                        owner.botMachine.ChangeState(AttackState.instance);
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
