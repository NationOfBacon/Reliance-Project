using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;

public class EnemyAttackState : EnemyState
{
    private static EnemyAttackState _instance; //only declared once

    private EnemyAttackState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyAttackState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyAttackState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner)
    {
        float timeWaitingToShoot = 0;
        bool movingCloserToEnemy = false;

        while (owner.botMachine.currentState == EnemyAttackState.instance)
        {
            if(!owner.disabled)
            {
                if (owner.enemyObject == null)
                {
                    owner.canSeeEnemy = false;
                    owner.health = null;
                    if (!owner.insideTurret)
                        owner.botMachine.ChangeState(EnemyChaseState.instance);
                    else
                        owner.botMachine.ChangeState(EnemyTurretSearchState.instance);
                    yield break;
                }

                if (owner.enemyObject != null)
                {
                    if (owner.turretBot == false && movingCloserToEnemy == false)
                        owner.thisAgent.SetDestination(owner.transform.position);
                    else
                        owner.GetComponent<Animator>().enabled = false;

                    //before firing check if there are too many enemies and if the bot should change to the retreat state
                    if (owner.additionalEnemies.Count >= 2 && owner.friendlyBots.Count <= 1 && !owner.turretBot)
                    {
                        for (int i = 0; i < owner.movingBotsOnly.Count; i++) //for all moving bots, check if any of them are close enough to retreat to
                        {
                            if (Vector3.Distance(owner.movingBotsOnly[i].transform.position, owner.transform.position) <= owner.retreatRadius && Vector3.Distance(owner.movingBotsOnly[i].transform.position, owner.transform.position) >= 50)
                            {
                                owner.botMachine.ChangeState(EnemyRetreatState.instance);
                                yield break;
                            }
                        }
                    }

                    int numberOfBotsCalled = 0;

                    for (int i = 0; i < owner.movingBotsOnly.Count; i++) //if there are any friendly bots within the reinforcementRadius, give them this bots target
                    {
                        if (Vector3.Distance(owner.transform.position, owner.movingBotsOnly[i].transform.position) <= owner.reinforcementRadius)
                        {
                            if (owner.movingBotsOnly[i].GetComponent<EnemyAIMachine>().enemyObject == null) //if the current bot doesnt already have a target, set it
                            {
                                owner.movingBotsOnly[i].GetComponent<EnemyAIMachine>().enemyObject = owner.enemyObject;
                                numberOfBotsCalled++;
                            }
                        }
                    }

                    if (numberOfBotsCalled != 0)
                    {
                        owner.logD.RecieveLog(owner.displayName + " called " + numberOfBotsCalled + " bot(s) for help!");
                        numberOfBotsCalled = 0;
                    }


                    if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= owner.attackDistance && owner.canSeeEnemy)
                    {
                        MoveTurret(owner);

                        timeWaitingToShoot += Time.deltaTime;

                        if (timeWaitingToShoot >= 5f)
                        {
                            owner.thisAgent.SetDestination(owner.enemyObject.transform.position);
                            movingCloserToEnemy = true;

                            if (Vector3.Distance(owner.transform.position, owner.enemyObject.transform.position) <= 20)
                            {
                                owner.thisAgent.SetDestination(owner.transform.position);
                                movingCloserToEnemy = false;
                            }
                        }

                        //after calling the MoveTurret method, wait for the onTarget bool to be true
                        if (owner.onTarget == true)
                        {
                            timeWaitingToShoot = 0;
                            owner.laserLine.SetPosition(0, owner.barrelEnd.position);
                            owner.laserLine.SetPosition(1, owner.contactPoint);

                            owner.laserLine.enabled = true;
                            owner.audioS.PlayOneShot(owner.GetComponent<MultipleAudioClips>().clips[1]);
                            yield return new WaitForSeconds(0.5f);
                            owner.laserLine.enabled = false;

                            Shoot(owner);

                            bool removing = false;

                            for (int i = 0; i < owner.friendlyBots.Count; i++) //if any of the bots are already removing an enemy, dont do anything
                            {
                                if (owner.friendlyBots[i].GetComponent<EnemyAIMachine>().removingEnemy == true)
                                {
                                    removing = true;
                                    break;
                                }
                            }

                            if (removing == false)
                            {
                                if (owner.enemyObject != null)
                                {
                                    if (owner.health.health <= 0)
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
                        if (!owner.insideTurret)
                            owner.botMachine.ChangeState(EnemyChaseState.instance);
                        else
                            owner.botMachine.ChangeState(EnemyTurretSearchState.instance);
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

    private void Shoot(EnemyAIMachine _owner)
    {
        int randomHitChance = Random.Range(0, 100);
        int randomCritChance = Random.Range(0, 100);

        if (!_owner.enemyObject.GetComponent<StructureInfo>())
        {
            int modifiedHitChance = 0;
            modifiedHitChance = _owner.enemyObject.GetComponent<BotStats>().ModifyEnemyHitChance(_owner.hitChance); //call to the enemys bot stats script to change the hit chance based on thier lucky stat

            if (randomHitChance <= modifiedHitChance && _owner.enemyObject != null)
            {
                _owner.audioS.PlayOneShot(_owner.enemyObject.GetComponent<MultipleAudioClips>().clips[0]);
                int damageAmt = 0;

                if(randomCritChance <= _owner.critChance)
                {
                    _owner.health.HealthReduce(2); //reduces health of target, health script grabbed in update of EnemyAIMachine
                    damageAmt = 2;
                }
                else
                {
                    _owner.health.HealthReduce();
                    damageAmt = 1;
                }

                if (_owner.enemyObject.GetComponent<AIMachine>() != null) //if the enemy is an player controlled AI
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<AIMachine>().displayName + " takes " + damageAmt + " damage");

                    if (_owner.enemyObject.GetComponent<AIMachine>().enemyObject == null) //if the enemy doesnt already have a target, set it to this bot
                    {
                        _owner.enemyObject.GetComponent<AIMachine>().enemyObject = _owner.gameObject;
                        _owner.enemyObject.GetComponent<AIMachine>().canSeeEnemy = true;
                    }
                }
                else if (_owner.enemyObject.GetComponent<AIAllyMachine>()) //if the enemy is an Ally AI
                {
                    _owner.logD.RecieveLog(_owner.displayName + ": Ally bot takes " + damageAmt + " damage");

                    if (_owner.enemyObject.GetComponent<AIAllyMachine>().enemyObject == null) //if the enemy doesnt already have a target, set it to this bot
                    {
                        _owner.enemyObject.GetComponent<AIAllyMachine>().enemyObject = _owner.gameObject;
                        _owner.enemyObject.GetComponent<AIAllyMachine>().canSeeEnemy = true;
                    }
                }
                else //if the enemy is the player
                    _owner.logD.RecieveLog(_owner.displayName + ": lead bot takes " + damageAmt + " damage");
            }
            else
            {
                _owner.logD.RecieveLog(_owner.displayName + " missed their shot.");
            }
        }
        else
        {
            if(randomHitChance <= _owner.hitChance && _owner.enemyObject != null)
            {
                _owner.audioS.PlayOneShot(_owner.enemyObject.GetComponent<MultipleAudioClips>().clips[0]);
                int damageAmt = 0;

                if (randomCritChance <= _owner.critChance)
                {
                    _owner.health.HealthReduce(2);
                    damageAmt = 2;
                }
                else
                {
                    _owner.health.HealthReduce();
                    damageAmt = 1;
                }

                _owner.logD.RecieveLog(_owner.displayName + ": " + _owner.enemyObject.GetComponent<StructureInfo>().displayName + "takes " + damageAmt + " damage");
            }
        }

    }

    private void OnEnemyDeath(EnemyAIMachine _owner)
    {
        var tempEnemyHolder = _owner.enemyObject;

        _owner.enemyObject.SetActive(false);
        _owner.enemyObject = null;
        _owner.health = null;
        _owner.canSeeEnemy = false;

        if (tempEnemyHolder.GetComponent<StructureInfo>()) //check what type of enemy was killed and put out the correct logs
        {
            _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<StructureInfo>().displayName + " has died", Color.red);
        }
        else
        {
            if (tempEnemyHolder.GetComponent<Health>().HP == null) //if the enemy target has no text variable, it is an ally (protect mode unit)
            {
                _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<AIAllyMachine>().displayName + " has died", Color.red);
            }
            else
            {
                tempEnemyHolder.GetComponent<Health>().HP.text = "Dead";

                if (tempEnemyHolder.GetComponent<AIMachine>() != null)
                    _owner.logD.RecieveLog(tempEnemyHolder.GetComponent<AIMachine>().displayName + " has died", Color.red);
                else
                    _owner.logD.RecieveLog(_owner.displayName + ": lead bot has died", Color.red);
            }
        }

        for (int i = 0; i < _owner.friendlyBots.Count; i++) //after the enemy is dead, check other friendly bots for the same target, remove it, then send them to an appropriate state
        {
            EnemyAIMachine targetBot = null;

            targetBot = _owner.friendlyBots[i].GetComponent<EnemyAIMachine>();

            if (targetBot.enemyObject == tempEnemyHolder)
            {
                targetBot.enemyObject = null;
                targetBot.health = null;
                targetBot.canSeeEnemy = false;

                var currentState = targetBot.botMachine.currentState;

                if (currentState == EnemyAttackState.instance && !_owner.turretBot)
                    targetBot.botMachine.ChangeState(EnemyChaseState.instance);
                else if (currentState == EnemyAttackState.instance && _owner.turretBot)
                    targetBot.botMachine.ChangeState(EnemyTurretSearchState.instance);

            }
        }

        _owner.removingEnemy = false;
    }

    private void MoveTurret(EnemyAIMachine _owner) //called to move the bots turret so that it faces the target
    {
        //declare variables that will be used to determine how to move the turret
        Vector3 hitPos = _owner.enemyObject.GetComponentInChildren<HighlightObject>().transform.position;
        Quaternion hitRot = Quaternion.LookRotation(hitPos - _owner.transform.position);
        Quaternion turretRot = Quaternion.LookRotation(hitPos - _owner.turretPos.transform.position);

        if(Vector3.Angle(_owner.turretPos.transform.forward, _owner.enemyObject.transform.position - _owner.turretPos.transform.position) > 5) //rotate the turret until it is facing within 5 degrees of the enemy
        {
            //rotate the turret over time to the enemy
            _owner.turretPos.transform.rotation = Quaternion.RotateTowards(_owner.turretPos.transform.rotation, turretRot, Time.deltaTime * _owner.turretRotateSpeed);

            //set the turrets y euler angle so that it only moves on that transform
            _owner.turretPos.transform.localEulerAngles = new Vector3(0, _owner.turretPos.transform.localEulerAngles.y, 0);
        }

        //rotate the barrel over time to the enemy
        _owner.barrelPos.transform.rotation = Quaternion.RotateTowards(_owner.barrelPos.transform.rotation, turretRot, Time.deltaTime * _owner.barrelMoveSpeed);

        //set the barrels y euler angle so that it only moves on that transform
        if(_owner.turretBot)
            _owner.barrelPos.transform.localEulerAngles = new Vector3(_owner.barrelPos.transform.localEulerAngles.x, 0, 0);
        else
            _owner.barrelPos.transform.localEulerAngles = new Vector3(0, _owner.barrelPos.transform.localEulerAngles.y, 0);
    }
}
