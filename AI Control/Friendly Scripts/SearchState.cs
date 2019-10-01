using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class SearchState : State
{
    private static SearchState _instance; //only declared once


    private SearchState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SearchState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new SearchState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        Waypoint currentWaypoint = null;
        Waypoint previousWaypoint = null;
        bool firstPointVisited = false;
        bool travelling = false;

        while (owner.botMachine.currentState == SearchState.instance)
        {
            if (!owner.disabled)
            {
                if (currentWaypoint == null)
                {
                    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
                    List<Waypoint> closeWaypoints = new List<Waypoint>();

                    if (allWaypoints.Length > 0)
                    {
                        while (currentWaypoint == null)
                        {
                            foreach (GameObject waypoint in allWaypoints)
                            {
                                if (Vector3.Distance(owner.gameObject.transform.position, waypoint.transform.position) <= 50)
                                {
                                    closeWaypoints.Add(waypoint.GetComponent<Waypoint>());
                                }
                            }

                            int random = Random.Range(0, closeWaypoints.Count);
                            Waypoint startingWaypoint = closeWaypoints[random];

                            if (startingWaypoint != null)
                            {
                                currentWaypoint = startingWaypoint;
                            }
                        }
                    }
                }

                if (firstPointVisited == true && travelling == false)
                {
                    Waypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
                    previousWaypoint = currentWaypoint;
                    currentWaypoint = nextWaypoint;
                }

                owner.targetWaypoint = currentWaypoint.transform.position;

                foreach (GameObject bot in owner.friendlyBots)
                {
                    if (bot != null && !bot.GetComponent<AIAllyMachine>())
                    {
                        var target = bot.GetComponent<AIMachine>().targetWaypoint;

                        if (owner.targetWaypoint == target)
                        {
                            Debug.Log(bot + "has the same target as another bot");
                            Waypoint nextWaypoint = currentWaypoint.NextWaypoint(previousWaypoint);
                            previousWaypoint = currentWaypoint;
                            currentWaypoint = nextWaypoint;
                        }
                    }
                }

                if (owner.targetWaypoint != null)
                {
                    travelling = true;

                    //check if the bot is already rotated before rotating the body
                    if (Vector3.Angle(owner.transform.forward, owner.targetWaypoint - owner.transform.position) > 10)
                    {
                        if (RotateBody(owner, owner.targetWaypoint)) //as it is, this makes the bot basically do a lookat command. This should stop happening after the bot is rotated
                        {
                            owner.thisAgent.SetDestination(owner.targetWaypoint);

                            Debug.DrawLine(owner.transform.position, owner.targetWaypoint, Color.blue);

                            if (travelling && owner.thisAgent.remainingDistance <= owner.markerDistance)
                            {
                                travelling = false;
                                firstPointVisited = true;
                            }
                        }
                    }
                    else
                    {
                        owner.thisAgent.SetDestination(owner.targetWaypoint);

                        Debug.DrawLine(owner.transform.position, owner.targetWaypoint, Color.blue);

                        if (travelling && owner.thisAgent.remainingDistance <= owner.markerDistance)
                        {
                            travelling = false;
                            firstPointVisited = true;
                        }
                    }

                }

                if (owner.canSeeEnemy && owner.enemyObject != null)
                {
                    owner.botMachine.ChangeState(ChaseState.instance);
                    yield break;
                }
                else
                {
                    owner.canSeeEnemy = false;
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
