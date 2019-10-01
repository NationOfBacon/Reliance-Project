using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class PartnerState : State
{
    private static PartnerState _instance; //only declared once

    private PartnerState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static PartnerState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new PartnerState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        while (owner.botMachine.currentState == PartnerState.instance)
        {
            if (!owner.disabled)
            {
                if (owner.followTarget != null) //if there is a follow target, follow it
                {
                    if (RotateBody(owner, owner.followTarget.transform.position))
                    {
                        owner.thisAgent.SetDestination(owner.followTarget.transform.position);

                        if (Vector3.Distance(owner.transform.position, owner.followTarget.transform.position) <= owner.followDistance) //if close to the target, stop
                        {
                            owner.thisAgent.SetDestination(owner.transform.position);
                        }
                    }

                    if (!owner.followTarget.activeSelf) //if the follow target is no longer active it is null
                    {
                        owner.followTarget = null;
                        owner.logD.RecieveLog(owner.displayName + "'s leader has died, reverting to idle", owner.gameObject);
                        owner.botMachine.ChangeState(IdleState.instance);
                        yield break;
                    }

                    if (owner.followTarget.GetComponent<AIMachine>().canSeeEnemy && owner.enemyObject == null) //if follow target has target and this bot does not, set the target to the follow targets target
                    {
                        owner.canSeeEnemy = true;
                        owner.enemyObject = owner.followTarget.GetComponent<AIMachine>().enemyObject;
                        owner.botMachine.ChangeState(ChaseState.instance);
                        yield break;
                    }

                    if (owner.canSeeEnemy && owner.enemyObject != null && Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance) //if this bot sees a target, attack it
                    {
                        owner.botMachine.ChangeState(AttackState.instance);
                        yield break;
                    }
                }
                else //if the follow target is null, go to the idle state
                {
                    owner.logD.RecieveLog(owner.displayName + "'s leader has died, reverting to idle", owner.gameObject);
                    owner.botMachine.ChangeState(IdleState.instance);
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
