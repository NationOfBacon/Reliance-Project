using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;

public class EnemyPartnerState : EnemyState
{
    private static EnemyPartnerState _instance; //only declared once

    private EnemyPartnerState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyPartnerState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyPartnerState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner) //no errors but not all leaders get followers and some bots wont move after spawning
    {
        bool followTargetFound = false;
        GameObject followTarget = null;

        while (owner.botMachine.currentState == EnemyPartnerState.instance)
        {
            if(!owner.disabled)
            {
                if (!followTargetFound) //check if the target has been found before entering the loop
                {
                    //check each bot for open slots and then set them as the follow target
                    for (int i = 0; i < owner.movingBotsOnly.Count; i++)
                    {
                        var targetBot = owner.movingBotsOnly[i].GetComponent<EnemyAIMachine>();

                        if (targetBot.leader == true) //check if the bot is a leader
                        {
                            if (targetBot.partnerBot == null) //if the first partner slot is empty, fill it and then break out of the for loop
                            {
                                targetBot.partnerBot = owner.gameObject;
                                followTarget = owner.movingBotsOnly[i];
                                followTargetFound = true;
                                break;
                            }

                            if (targetBot.partnerBot1 == null) //if the second partner slot is empty, fill it and then break out of the loop
                            {
                                targetBot.partnerBot1 = owner.gameObject;
                                followTarget = owner.movingBotsOnly[i];
                                followTargetFound = true;
                                break;
                            }
                        }
                    }
                    if (!followTargetFound || followTarget == null || !followTarget.activeSelf) //if a follow target is not found after checking each bot, revert to regular searching
                    {
                        followTarget = null;
                        followTargetFound = false;

                        owner.insidePartner = false;
                        owner.insideNormalSearch = true;
                        owner.botMachine.ChangeState(EnemySearchState.instance);
                    }
                }

                if (followTargetFound == true && followTarget != null && followTarget.activeSelf) //check if the target exists and is active
                {
                    if (RotateBody(owner, followTarget.transform.position))
                    {
                        owner.thisAgent.SetDestination(followTarget.transform.position);

                        //if the bot is too close to the target, stop where you are
                        if (Vector3.Distance(owner.transform.position, followTarget.transform.position) <= owner.followDistance)
                        {
                            owner.thisAgent.SetDestination(owner.transform.position);
                        }
                    }

                    //if the bot finds an enemy while following, chase it
                    if (owner.canSeeEnemy && owner.enemyObject != null)
                    {
                        //enter the partner chase state
                        owner.botMachine.ChangeState(EnemyChaseState.instance);
                    }
                }

                if (followTarget == null || followTargetFound == false || !followTarget.activeSelf) //if the target is null or inactive, reset the variables
                {
                    followTarget = null;
                    followTargetFound = false;

                    owner.insidePartner = false;
                    owner.insideNormalSearch = true;
                    owner.botMachine.ChangeState(EnemySearchState.instance);
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
