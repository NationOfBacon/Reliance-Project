using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class HealState : State
{
    private static HealState _instance; //only declared once

    private HealState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static HealState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new HealState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        while (owner.botMachine.currentState == HealState.instance)
        {
            if (!owner.disabled)
            {
                for (int i = 0; i < owner.friendlyBots.Count; i++) //for each friendly bot
                {
                    if (owner.friendlyBots[i].GetComponent<Health>().health < owner.friendlyBots[i].GetComponent<Health>().maxHealth) //if the current friendly bots HP is less than max
                    {
                        if (RotateBody(owner, owner.friendlyBots[i].transform.position))
                        {
                            owner.thisAgent.SetDestination(owner.friendlyBots[i].transform.position); //move to the current friendly bot

                            if (Vector3.Distance(owner.transform.position, owner.friendlyBots[i].transform.position) <= owner.healDistance) //if the distance between this bot and the friendly bot is less than the heal distance
                            {
                                owner.thisAgent.SetDestination(owner.transform.position); //set the destination to itself so that it stops
                                Debug.Log("In position to heal");

                                if (owner.friendlyBots[i].activeSelf) //if the bot is still active when the heal bot is in range, begin healing
                                {
                                    owner.laserLine.SetPosition(0, owner.transform.position);
                                    owner.laserLine.SetPosition(1, owner.friendlyBots[i].transform.position);
                                    owner.laserLine.gameObject.GetComponent<Renderer>().material.color = Color.green;

                                    do
                                    {
                                        Debug.Log("Healing");
                                        owner.laserLine.enabled = true;
                                        owner.audioS.PlayOneShot(owner.GetComponent<MultipleAudioClips>().clips[2]);
                                        yield return new WaitForSeconds(owner.healRate);
                                        owner.laserLine.enabled = false;

                                        owner.friendlyBots[i].GetComponent<Health>().HealthIncrease();
                                        owner.logD.RecieveLog(owner.displayName + ": " + owner.friendlyBots[i].GetComponent<AIMachine>().displayName + " was healed by 1 point", owner.gameObject);
                                    }
                                    while (owner.friendlyBots[i].GetComponent<Health>().health < owner.friendlyBots[i].GetComponent<Health>().maxHealth && owner.friendlyBots[i].activeSelf);
                                }
                            }
                        }
                    }
                }

                if (owner.playerObject.GetComponent<Health>().health < owner.playerObject.GetComponent<Health>().maxHealth) //do the same as above but for the player bot
                {
                    if (RotateBody(owner, owner.playerObject.transform.position))
                    {
                        owner.thisAgent.SetDestination(owner.playerObject.transform.position);

                        if (Vector3.Distance(owner.transform.position, owner.playerObject.transform.position) <= owner.healDistance)
                        {
                            owner.thisAgent.SetDestination(owner.transform.position);
                            Debug.Log("In position to heal");
                            owner.thisAgent.transform.LookAt(owner.playerObject.transform);

                            if (owner.playerObject.activeSelf) //if the player object is still active, begin healing
                            {
                                owner.laserLine.SetPosition(0, owner.transform.position);
                                owner.laserLine.SetPosition(1, owner.playerObject.transform.position);
                                owner.laserLine.gameObject.GetComponent<Renderer>().material.color = Color.green;

                                do
                                {
                                    Debug.Log("Healing");
                                    owner.laserLine.enabled = true;
                                    owner.audioS.PlayOneShot(owner.GetComponent<MultipleAudioClips>().clips[2]);
                                    yield return new WaitForSeconds(owner.healRate);
                                    owner.laserLine.enabled = false;

                                    owner.playerObject.GetComponent<Health>().HealthIncrease();
                                    owner.logD.RecieveLog(owner.displayName + ": " + owner.playerObject.name + " was healed by 1 point", owner.gameObject);
                                }
                                while (owner.playerObject.GetComponent<Health>().health < owner.playerObject.GetComponent<Health>().maxHealth && owner.playerObject.activeSelf);
                            }
                        }
                    }
                }

                Debug.Log("Healing cycle complete");
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