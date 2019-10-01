using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;
using UnityEngine.AI;

public class SupportState : State
{
    private static SupportState _instance; //only declared once

    private SupportState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SupportState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new SupportState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        while (owner.botMachine.currentState == SupportState.instance)
        {
            if (!owner.disabled)
            {
                GameObject targetBot = null;

                //in this state the bot will locate friendly bots that are not buffed (via a bool) and then launch grenades with an AOE buff to their location
                for (int i = 0; i < owner.friendlyBots.Count; i++)
                {
                    if (!owner.friendlyBots[i].GetComponent<AIMachine>().supportBuff)
                    {
                        targetBot = owner.friendlyBots[i];
                        break;
                    }
                }

                if (targetBot != null)
                {
                    //if not buffed, get in range to buff
                    if (Vector3.Distance(owner.transform.position, targetBot.transform.position) >= owner.healDistance)
                    {
                        if (RotateBody(owner, targetBot.transform.position))
                        {
                            owner.thisAgent.SetDestination(targetBot.transform.position);
                        }
                    }
                    else
                    {
                        MoveTurret(owner, targetBot);

                        if (owner.onTarget)
                        {
                            owner.thisAgent.SetDestination(owner.transform.position);
                            //once in range to buff, launch grenade (grenade will be a prefab object)
                            var grenadeCopy = GameObject.Instantiate(owner.grenadePrefab as GameObject, owner.transform);
                            grenadeCopy.SetActive(true);
                            //generate a sphere around the target bot and chose a random location within it for the grenade to impact
                            Vector3 landArea = GenerateLandingPoint(owner, targetBot);

                            //have the grenade travel over time to the destination. Look at https://lunarlabs.pt/blog/post/the_art_of_lerp for help (get code working then impliment lerp after)
                            yield return new WaitForSeconds(2.0f);
                            grenadeCopy.transform.position = landArea;
                            //once the grenade impacts, generate another sphere around the impact location that will be the AOE buff zone
                            //instantiate another object that will be the buff area and have a script on it to handle buffing
                            var areaPrefab = GameObject.Instantiate(owner.areaHealPrefab as GameObject, grenadeCopy.transform);
                            areaPrefab.transform.parent = grenadeCopy.transform.parent.parent;
                            areaPrefab.GetComponent<AreaBuff>().grenadeObject = grenadeCopy;
                        }

                        owner.botMachine.ChangeState(IdleState.instance);
                    }
                }
            else
            {
                owner.thisAgent.SetDestination(owner.transform.position);
            }

                yield return null;
            }
        }
    }

    public Vector3 GenerateLandingPoint(AIMachine _owner, GameObject targetBot)
    {
        NavMeshHit NH;

        Vector3 point = targetBot.transform.position + Random.insideUnitSphere * 20; //create a point on the navmesh
        NavMesh.SamplePosition(point, out NH, 20, NavMesh.AllAreas); //create a position to give to the NH var

        return NH.position;
    }

    private bool RotateBody(AIMachine _owner, Vector3 navPoint) //called to have the bot rotate to face the waypoint before navigating to it
    {
        //declare variables that will be used to determine how to move the bot
        Quaternion targetRot = Quaternion.LookRotation(navPoint - _owner.transform.position);

        //rotate the bot over time to face the enemy
        _owner.transform.rotation = Quaternion.RotateTowards(_owner.transform.rotation, targetRot, Time.deltaTime * 25f);

        //set the bots y euler angle so that it only moves on that transform
        _owner.transform.localEulerAngles = new Vector3(0, _owner.transform.localEulerAngles.y, 0);


        if (Vector3.Angle(_owner.transform.forward, navPoint - _owner.transform.position) < 10)
            return true;
        else
            return false;
    }

    private void MoveTurret(AIMachine _owner, GameObject targetBot) //called to move the bots turret so that it faces the target
    {
        //declare variables that will be used to determine how to move the turret
        Vector3 hitPos = targetBot.GetComponent<HighlightObject>().transform.gameObject.transform.position;
        Quaternion hitRot = Quaternion.LookRotation(hitPos - _owner.transform.position);
        Quaternion turretRot = Quaternion.LookRotation(hitPos - _owner.turretPos.transform.position);

        if (Vector3.Angle(_owner.turretPos.transform.forward, _owner.enemyObject.transform.position - _owner.turretPos.transform.position) > 5) //rotate the turret until it is facing within 5 degrees of the enemy
        {
            //rotate the turret over time to the enemy
            _owner.turretPos.transform.rotation = Quaternion.RotateTowards(_owner.turretPos.transform.rotation, turretRot, Time.deltaTime * _owner.turretRotateSpeed);

            //set the turrets y euler angle so that it only moves on that transform
            _owner.turretPos.transform.localEulerAngles = new Vector3(0, _owner.turretPos.transform.localEulerAngles.y, 0);
        }

        //rotate the barrel over time to the enemy
        _owner.barrelPos.transform.rotation = Quaternion.RotateTowards(_owner.barrelPos.transform.rotation, turretRot, Time.deltaTime * _owner.barrelMoveSpeed);

        //set the barrels y euler angle so that it only moves on that transform
        _owner.barrelPos.transform.localEulerAngles = new Vector3(0, _owner.barrelPos.transform.localEulerAngles.y, 0);
    }
}
