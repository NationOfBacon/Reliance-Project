using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;
using UnityEngine.AI;

public class EnemySearchState : EnemyState
{
    private static EnemySearchState _instance; //only declared once

    private EnemySearchState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemySearchState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemySearchState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner)
    {
        Vector3 point = MoveWaypoint(owner);
        float waitTime = 3f;
        float elapsedTime = 0f;

        while (owner.botMachine.currentState == EnemySearchState.instance)
        {
            if(!owner.disabled)
            {
                //before setting the destination, have the bot rotate to face the point
                if (RotateBody(owner, point))
                {
                    owner.thisAgent.SetDestination(point); //set the bots destination to the generated point

                    if (Vector3.Distance(owner.gameObject.transform.position, point) <= 5) //if the bot gets within 5 units of a destination, stop and wait
                    {
                        owner.thisAgent.SetDestination(owner.gameObject.transform.position); //set destination to itself

                        elapsedTime += Time.deltaTime; //track time

                        if (elapsedTime >= waitTime) //if the elapsed time goes over the wait time, generate a new waypoint
                        {
                            elapsedTime = 0f;
                            point = MoveWaypoint(owner);
                        }
                    }
                }

                if (owner.canSeeEnemy && owner.enemyObject != null) //if an enemy is found, go to the chase state
                {
                    owner.botMachine.ChangeState(EnemyChaseState.instance);
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

    public Vector3 MoveWaypoint(EnemyAIMachine _owner) //generate a position within the search radius
    {
        RaycastHit hit;
        NavMeshHit NH;

        Vector3 point = _owner.transform.position + Random.insideUnitSphere * _owner.searchRadius; //create a point on the navmesh
        NavMesh.SamplePosition(point, out NH, _owner.searchRadius, NavMesh.AllAreas); //create a position to give to the NH var

        if (Physics.Raycast(_owner.lineOrigin.transform.position, NH.position - _owner.transform.position, out hit, _owner.searchRadius)) //draw a ray from this bot to the point at the same length as the distance from the bot to the point
        {
            //if the ray hits any object, get a new point and redo the raycast until the cast is false
            do
            {
                point = _owner.transform.position + Random.insideUnitSphere * _owner.searchRadius;
                NavMesh.SamplePosition(point, out NH, _owner.searchRadius, NavMesh.AllAreas);
            }
            while (Physics.Raycast(_owner.lineOrigin.transform.position, NH.position - _owner.transform.position, out hit, _owner.searchRadius));
        }

        //show rays from the bots position to the point
        Debug.DrawLine(_owner.gameObject.transform.position, NH.position, Color.cyan, 5);
        Debug.DrawRay(_owner.lineOrigin.transform.position, NH.position - _owner.transform.position, Color.magenta, 5);

        return NH.position;
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
