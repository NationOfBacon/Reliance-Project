using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class MoveToState : State
{
    private static MoveToState _instance; //only declared once

    private MoveToState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static MoveToState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new MoveToState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        bool locationGet = false; //bool used to figure out if the bot has a location before navigating to it

        while (owner.botMachine.currentState == MoveToState.instance)
        {
            if (!owner.disabled)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && locationGet == false) //if the right mouse button is pressed and the location is not found yet, get a position
                {
                    owner.MarkLocation();
                    locationGet = true;
                }

                if (locationGet == true) //if the location is found, go to it
                {
                    if (RotateBody(owner, owner.marker.transform.position))
                    {
                        owner.thisAgent.SetDestination(owner.marker.transform.position);

                        if (Vector3.Distance(owner.transform.position, owner.marker.transform.position) <= owner.moveToDistance)
                        {
                            owner.RemoveMarker(); //get rid of the marker if it is not needed anymore
                            owner.botMachine.ChangeState(IdleState.instance);
                            yield break;
                        }

                        if (owner.canSeeEnemy && owner.enemyObject != null)
                        {
                            owner.RemoveMarker();
                            owner.botMachine.ChangeState(ChaseState.instance);
                            yield break;
                        }
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
