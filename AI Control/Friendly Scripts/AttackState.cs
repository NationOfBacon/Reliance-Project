using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class AttackState : State
{
    private static AttackState _instance; //only declared once

    private AttackState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AttackState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        float timeWaitingToShoot = 0;
        bool movingCloserToEnemy = false;

        while (owner.botMachine.currentState == AttackState.instance)
        {
            if (!owner.disabled)
            {
                if (owner.enemyObject == null)
                {
                    owner.canSeeEnemy = false;
                    owner.enemyHealth = null;

                    LeaveState(owner);
                    yield break;
                }
                else
                {
                    //before firing check if there are too many enemies and if the bot should change to the retreat state
                    if (owner.additionalEnemies.Count >= 2 && owner.nearbyFriendlies.Count <= 1)
                    {
                        for (int i = 0; i < owner.friendlyBots.Count; i++) //for all moving bots, check if any of them are close enough to retreat to
                        {
                            if (Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) <= owner.retreatRadius && Vector3.Distance(owner.friendlyBots[i].transform.position, owner.transform.position) >= 50)
                            {
                                owner.botMachine.ChangeState(RetreatState.instance);
                                yield break;
                            }
                        }
                    }

                    int numberOfBotsCalled = 0;

                    for (int i = 0; i < owner.friendlyBots.Count; i++) //if there are any friendly bots within the reinforcementRadius, give them this bots target
                    {
                        if (Vector3.Distance(owner.transform.position, owner.friendlyBots[i].transform.position) <= owner.reinforcementRadius)
                        {
                            if (owner.friendlyBots[i].GetComponent<AIMachine>().enemyObject == null) //if the current bot doesnt already have a target, set it
                            {
                                owner.friendlyBots[i].GetComponent<AIMachine>().enemyObject = owner.enemyObject;
                                numberOfBotsCalled++;
                            }
                        }
                    }

                    if (numberOfBotsCalled != 0)
                    {
                        owner.logD.RecieveLog(owner.displayName + " called " + numberOfBotsCalled + " bot(s) for help!");
                        numberOfBotsCalled = 0;
                    }

                    if(movingCloserToEnemy == false)
                        owner.thisAgent.SetDestination(owner.transform.position);

                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy)
                    {
                        MoveTurret(owner, owner.enemyObject.GetComponentInChildren<HighlightObject>().gameObject.transform.position);

                        timeWaitingToShoot += Time.deltaTime;

                        if(timeWaitingToShoot >= 5f)
                        {
                            owner.thisAgent.SetDestination(owner.enemyObject.transform.position);
                            movingCloserToEnemy = true;

                            if(Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= 20)
                            {
                                owner.thisAgent.SetDestination(owner.transform.position);
                                movingCloserToEnemy = false;
                            }
                        }

                        if (owner.onTarget == true)
                        {
                            timeWaitingToShoot = 0;

                            owner.laserLine.SetPosition(0, owner.barrelEnd.position);
                            owner.laserLine.SetPosition(1, owner.contactPoint);

                            owner.laserLine.enabled = true;
                            owner.audioS.PlayOneShot(owner.GetComponent<MultipleAudioClips>().clips[1]);
                            yield return new WaitForSeconds(owner.fireRate);
                            owner.laserLine.enabled = false;

                            Shoot(owner);

                            bool removing = false;

                            for (int i = 0; i < owner.friendlyBots.Count; i++) //if any of the bots are already removing an enemy, dont do anything
                            {
                                if (owner.friendlyBots[i].GetComponent<AIMachine>().removingEnemy == true)
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
                    else //if the enemy is out of attack range, chase after them
                    {
                        LeaveState(owner);
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

    private void Shoot(AIMachine _owner)
    {
        int randomHitChance = Random.Range(0, 100); //use a random int to determine if the shot will land

        if (randomHitChance <= _owner.hitChance && _owner.enemyObject != null)
        {
            int randomCritChance = Random.Range(0, 100);
            int randomHeavyHitChance = Random.Range(0, 100);

            _owner.enemyObject.GetComponent<AudioSource>().PlayOneShot(_owner.enemyObject.GetComponent<MultipleAudioClips>().clips[0]); //play the hit sound on the enemy bot

            if (randomCritChance <= _owner.critChance) //use a second random int to determine if the shot will be a critical hit
            {
                int damageAmt = 2;

                if (randomHeavyHitChance <= _owner.heavyHitChance)
                    damageAmt += _owner.heavyHitModifier;

                _owner.enemyHealth.HealthReduce(damageAmt);

                if (_owner.enemyObject.GetComponent<StructureInfo>()) //check if the enemy is a structure before giving the log
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<StructureInfo>().displayName + " takes " + damageAmt + " damage, CRITICAL!!!", _owner.gameObject);
                }
                else
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<EnemyAIMachine>().displayName + " takes " + damageAmt + " damage, CRITICAL!!!", _owner.gameObject);
                }
            }
            else
            {
                int damageAmt = 1;

                if (randomHeavyHitChance <= _owner.heavyHitChance)
                    damageAmt += _owner.heavyHitModifier;

                 _owner.enemyHealth.HealthReduce(damageAmt);

                if (_owner.enemyObject.GetComponent<StructureInfo>())
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<StructureInfo>().displayName + " takes " + damageAmt + " damage", _owner.gameObject);
                }
                else
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<EnemyAIMachine>().displayName + " takes " + damageAmt + " damage", _owner.gameObject);
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
        }
        else
        {
            _owner.logD.RecieveLog(_owner.displayName + " missed.");
        }
    }

    private void OnEnemyDeath(AIMachine _owner)
    {
        var tempEnemyHolder = _owner.enemyObject; //create a temp var to hold the enemy object so that it can be removed from this bot while still holding the reference

        _owner.enemyObject = null;
        _owner.canSeeEnemy = false;
        _owner.enemyHealth = null;

        if (tempEnemyHolder.GetComponent<StructureInfo>()) //if the enemy destroyed was a structure
        {
            _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<StructureInfo>().displayName + " was destroyed", Color.red);
            tempEnemyHolder.SetActive(false);
            _owner.botStats.KillExpIncrease(tempEnemyHolder);
        }
        else //if the enemy destroyed was a bot
        {
            _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<EnemyAIMachine>().displayName + " was destroyed", Color.red);
            tempEnemyHolder.SetActive(false);
            _owner.gameManager.OnKill(tempEnemyHolder);
            _owner.botStats.KillExpIncrease(tempEnemyHolder);
        }

        for (int i = 0; i < _owner.friendlyBots.Count; i++) //after the enemy is dead, check other friendly bots for the same target, remove it, then send them to an appropriate state
        {
            AIMachine targetBot = null;
            AIAllyMachine targetAlly = null;

            if (_owner.friendlyBots[i].GetComponent<BotStats>()) //if the bot is in the list but doesnt have botstats, it is an ally bot
                targetBot = _owner.friendlyBots[i].GetComponent<AIMachine>();
            else
                targetAlly = _owner.friendlyBots[i].GetComponent<AIAllyMachine>();

            if (targetAlly == null)
            {
                if (targetBot.enemyObject == tempEnemyHolder)
                {
                    targetBot.enemyObject = null;
                    targetBot.enemyHealth = null;
                    targetBot.canSeeEnemy = false;

                    var currentState = targetBot.botMachine.baseState;

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
                        targetAlly.botMachine.ChangeState(AllyRetreatState.instance);
                }
            }
        }

        _owner.removingEnemy = false;
    }

    private void MoveTurret(AIMachine _owner, Vector3 hitPos) //called to move the bots turret so that it faces whatever Vector3 is sent
    {
        //declare variables that will be used to determine how to move the turret
        Quaternion hitRot = Quaternion.LookRotation(hitPos - _owner.transform.position);
        Quaternion turretRot = Quaternion.LookRotation(hitPos - _owner.turretPos.transform.position);

        if (Vector3.Angle(_owner.turretPos.transform.forward, _owner.enemyObject.transform.position - _owner.turretPos.transform.position) > 5) //rotate the turret until it is facing within 5 degrees of the target
        {
            //rotate the turret over time to the target
            _owner.turretPos.transform.rotation = Quaternion.RotateTowards(_owner.turretPos.transform.rotation, turretRot, Time.deltaTime * _owner.turretRotateSpeed);

            //set the turrets y euler angle so that it only moves on that transform
            _owner.turretPos.transform.localEulerAngles = new Vector3(0, _owner.turretPos.transform.localEulerAngles.y, 0);
        }

        //rotate the barrel over time to the target
        _owner.barrelPos.transform.rotation = Quaternion.RotateTowards(_owner.barrelPos.transform.rotation, turretRot, Time.deltaTime * _owner.barrelMoveSpeed);

        //set the barrels y euler angle so that it only moves on that transform
        _owner.barrelPos.transform.localEulerAngles = new Vector3(0, _owner.barrelPos.transform.localEulerAngles.y, 0);
    }

    private void LeaveState(AIMachine _owner)
    {
        if (_owner.botMachine.baseState == SearchState.instance)
            _owner.botMachine.ChangeState(ChaseState.instance);
        else if (_owner.botMachine.baseState == CaptureAreaState.instance)
            _owner.botMachine.ChangeState(ChaseState.instance);
        else if (_owner.botMachine.baseState == FollowState.instance)
            _owner.botMachine.ChangeState(FollowState.instance);
        else if (_owner.botMachine.baseState == PartnerState.instance)
            _owner.botMachine.ChangeState(ChaseState.instance);
        else if (_owner.botMachine.baseState == MoveToState.instance)
            _owner.botMachine.ChangeState(ChaseState.instance);
        else if (_owner.botMachine.baseState == SentryState.instance)
            _owner.botMachine.ChangeState(SentryState.instance);
    }

}
