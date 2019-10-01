using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllyAIMachineTools;

public class AllyAttackState : AllyState
{
    private static AllyAttackState _instance; //only declared once

    private AllyAttackState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AllyAttackState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new AllyAttackState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIAllyMachine owner)
    {
        while (owner.botMachine.currentState == AllyAttackState.instance)
        {
            if(!owner.disabled)
            {
                if (owner.enemyObject == null)
                {
                    owner.canSeeEnemy = false;
                    owner.enemyHealth = null;
                    owner.botMachine.ChangeState(AllyChaseState.instance); //go to a retreat state
                    yield break;
                }
                else
                {
                    //before firing check if there are too many enemies and if there are any nearby allies to decide whether to retreat or not
                    if (owner.additionalEnemies.Count >= 2 && owner.nearbyFriendlies.Count <= 1)
                    {
                        for (int i = 0; i < owner.friendlyBots.Count; i++) //for all moving bots, check if any of them are close enough to retreat to
                        {
                            if (Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) <= owner.retreatRadius && Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) >= 50)
                            {
                                owner.botMachine.ChangeState(AllyRetreatState.instance);
                                yield break;
                            }
                        }
                    }

                    int numberOfBotsCalled = 0;

                    for (int i = 0; i < owner.friendlyBots.Count; i++) //if there are any friendly bots within the reinforcementRadius, give them this bots target
                    {
                        if (Vector3.Distance(owner.transform.position, owner.friendlyBots[i].transform.position) <= owner.reinforcementRadius)
                        {
                            if (owner.friendlyBots[i].GetComponent<AIAllyMachine>().enemyObject == null) //if the current bot doesnt already have a target, set it
                            {                                      
                                owner.friendlyBots[i].GetComponent<AIAllyMachine>().enemyObject = owner.enemyObject;
                                numberOfBotsCalled++;
                            }
                        }
                    }

                    if (numberOfBotsCalled != 0)
                    {
                        owner.logD.RecieveLog(owner.displayName + " called " + numberOfBotsCalled + " bot(s) for help!");
                        numberOfBotsCalled = 0;
                    }

                    owner.thisAgent.SetDestination(owner.transform.position);

                    owner.laserLine.SetPosition(0, owner.barrelEnd.position);
                    owner.laserLine.SetPosition(1, owner.contactPoint);

                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy)
                    {
                        MoveTurret(owner);

                        if (owner.onTarget == true)
                        {
                            owner.laserLine.enabled = true;
                            owner.audioS.PlayOneShot(owner.GetComponent<MultipleAudioClips>().clips[1]);
                            yield return new WaitForSeconds(0.5f);
                            owner.laserLine.enabled = false;

                            Shoot(owner);

                            bool removing = false;

                            for (int i = 0; i < owner.friendlyBots.Count; i++) //if any of the bots are already removing an enemy, dont do anything
                            {
                                if (owner.friendlyBots[i].GetComponent<AIAllyMachine>().removingEnemy == true)
                                {
                                    removing = true;
                                    break;
                                }
                            }

                            if (removing == false)
                            {
                                if (owner.enemyObject != null)
                                {
                                    if (owner.enemyHealth.health <= 0)
                                    {
                                        owner.removingEnemy = true;
                                        OnEnemyDeath(owner);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        owner.botMachine.ChangeState(AllyChaseState.instance); //go to a retreat state
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

    private void Shoot(AIAllyMachine _owner)
    {
        int random = Random.Range(0, 100); //use a random int to determine if the shot will land

        if (random <= _owner.hitChance && _owner.enemyObject != null)
        {
            int randomTwo = Random.Range(0, 100);

            if (randomTwo <= _owner.hitChance - 65) //use a second random int to determine if the shot will be a critical hit
            {
                _owner.enemyHealth.HealthReduce(2);

                if (_owner.enemyObject.GetComponent<StructureInfo>()) //check if the enemy is a structure before giving the log
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<StructureInfo>().displayName + " takes 2 damage, CRITICAL!!!", _owner.gameObject);
                }
                else
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<EnemyAIMachine>().displayName + " takes 2 damage, CRITICAL!!!", _owner.gameObject);
                }
            }
            else
            {
                _owner.enemyHealth.HealthReduce(); //reduces health of target, health script grabbed in update of AIMachine

                if (_owner.enemyObject.GetComponent<StructureInfo>())
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<StructureInfo>().displayName + " takes 1 damage", _owner.gameObject);
                }
                else
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<EnemyAIMachine>().displayName + " takes 1 damage", _owner.gameObject);
                }
            }

            if (!_owner.enemyObject.GetComponent<StructureInfo>()) //only execute if the enemy is not a structure
            {
                if (_owner.enemyObject.GetComponent<EnemyAIMachine>().enemyObject == null) //if the enemy you are firing at does not have a target, set its target to you
                {
                    _owner.enemyObject.GetComponent<EnemyAIMachine>().enemyObject = _owner.gameObject; //set the enemies enemyObject to this bot so that it will fight back
                    _owner.enemyObject.GetComponent<EnemyAIMachine>().canSeeEnemy = true;
                }
            }

            _owner.enemyObject.GetComponent<AudioSource>().PlayOneShot(_owner.enemyObject.GetComponent<MultipleAudioClips>().clips[0]); //play the hit sound on the enemy bot
        }
    }

    private void OnEnemyDeath(AIAllyMachine _owner)
    {
        var tempEnemyHolder = _owner.enemyObject; //create a temp var to hold the enemy object so that it can be removed from this bot while still holding the reference

        _owner.enemyObject = null;
        _owner.canSeeEnemy = false;
        _owner.enemyHealth = null;

        if (tempEnemyHolder.GetComponent<StructureInfo>()) //if the enemy destroyed was a structure
        {
            _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<StructureInfo>().displayName + " was destroyed", Color.red);
            tempEnemyHolder.SetActive(false);
        }
        else //if the enemy destroyed was a bot
        {
            _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<EnemyAIMachine>().displayName + " was destroyed", Color.red);
            tempEnemyHolder.SetActive(false);
            _owner.gameManager.OnKill(tempEnemyHolder);
        }

        for (int i = 0; i < _owner.friendlyBots.Count; i++) //after the enemy is dead, check other friendly bots for the same target, remove it, then send them to an appropriate state
        {
            AIMachine targetBot = null;
            AIAllyMachine targetAlly = null;

            if (_owner.friendlyBots[i].GetComponent<BotStats>()) //if the bot is in the list but doesnt have botstats, it is an ally bot
                targetBot = _owner.friendlyBots[i].GetComponent<AIMachine>();
            else
                targetAlly = _owner.friendlyBots[i].GetComponent<AIAllyMachine>();

            if(targetAlly == null)
            {
                if (targetBot.enemyObject == tempEnemyHolder)
                {
                    targetBot.enemyObject = null;
                    targetBot.enemyHealth = null;
                    targetBot.canSeeEnemy = false;

                    var currentState = targetBot.botMachine.currentState;

                    if (currentState == SearchState.instance)
                        targetBot.botMachine.ChangeState(ChaseState.instance);
                    else if (currentState == CaptureAreaState.instance)
                        targetBot.botMachine.ChangeState(ChaseState.instance);
                    else if (currentState == FollowState.instance)
                        targetBot.botMachine.ChangeState(FollowState.instance);
                    else if (currentState == PartnerState.instance)
                        targetBot.botMachine.ChangeState(ChaseState.instance);
                    else if (currentState == MoveToState.instance)
                        targetBot.botMachine.ChangeState(ChaseState.instance);
                    else if (currentState == SentryState.instance)
                        targetBot.botMachine.ChangeState(SentryState.instance);
                }
            }
            else
            {
                if (targetAlly.enemyObject == tempEnemyHolder)
                {
                    targetAlly.enemyObject = null;
                    targetAlly.enemyHealth = null;
                    targetAlly.canSeeEnemy = false;

                    var currentState = targetAlly.botMachine.currentState;

                    if (currentState != AllyRetreatState.instance)
                        targetAlly.botMachine.ChangeState(AllyChaseState.instance);
                }
            }
        }
    }

    private void MoveTurret(AIAllyMachine _owner) //called to move the bots turret so that it faces the target
    {
        //declare variables that will be used to determine how to move the turret
        Vector3 hitPos = _owner.enemyObject.GetComponentInChildren<HighlightObject>().gameObject.transform.position;
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
