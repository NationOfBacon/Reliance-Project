using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class CaptureAreaState : State
{
    private static CaptureAreaState _instance; //only declared once

    private CaptureAreaState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static CaptureAreaState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new CaptureAreaState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        while (owner.botMachine.currentState == CaptureAreaState.instance) //needs to navigate to the closest objective rather than any objective
        {
            if (!owner.disabled)
            {
                GameObject targetObj = null;

                foreach (GameObject obj in owner.objectives)
                {
                    targetObj = obj;

                    if (RotateBody(owner, targetObj.transform.position))
                    {
                        owner.thisAgent.SetDestination(targetObj.transform.position);
                        break;
                    }
                }

                if (Vector3.Distance(owner.transform.position, targetObj.transform.position) <= owner.followDistance)
                {
                    owner.thisAgent.SetDestination(owner.transform.position);
                }

                if (owner.canSeeEnemy && owner.enemyObject != null)
                {
                    owner.botMachine.ChangeState(ChaseState.instance); //needs a special chase state so that it will go back to the objective after fighting
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
