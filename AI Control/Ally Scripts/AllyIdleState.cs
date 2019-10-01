using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllyAIMachineTools;

public class AllyIdleState : AllyState
{
    private static AllyIdleState _instance; //only declared once

    private AllyIdleState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static AllyIdleState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new AllyIdleState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIAllyMachine owner)
    {
        float waitTime = 2f;
        float elapsedTime = 0f;
        float randomRotation = Random.Range(-20, 20);

        while (owner.botMachine.currentState == AllyIdleState.instance)
        {
            if(!owner.disabled)
            {
                owner.thisAgent.SetDestination(owner.transform.position);

                elapsedTime += Time.deltaTime;

                if (elapsedTime >= waitTime)
                {
                    elapsedTime = 0;
                    owner.transform.Rotate(0, randomRotation, 0);
                }

                if (owner.canSeeEnemy && owner.enemyObject != null)
                {
                    owner.botMachine.ChangeState(AllyChaseState.instance);
                    yield break;
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
