using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIMachineTools;
using UnityEngine.AI;

public class EnemyTurretSearchState : EnemyState
{
    private static EnemyTurretSearchState _instance; //only declared once

    private EnemyTurretSearchState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static EnemyTurretSearchState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new EnemyTurretSearchState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(EnemyAIMachine owner)
    {
        while (owner.botMachine.currentState == EnemyTurretSearchState.instance)
        {
            if(!owner.disabled)
            {
                owner.GetComponent<Animator>().enabled = true;

                if (owner.canSeeEnemy && owner.enemyObject != null)
                {
                    owner.botMachine.ChangeState(EnemyAttackState.instance);
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
}
