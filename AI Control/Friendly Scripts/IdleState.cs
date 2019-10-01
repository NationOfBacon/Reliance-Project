using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class IdleState : State
{
    private static IdleState _instance; //only declared once

    private IdleState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        float waitTime = 2f;
        float elapsedTime = 0f;
        float randomRotation = Random.Range(-20, 20);

        while (owner.botMachine.currentState == IdleState.instance)
        {
            if (!owner.disabled)
            {
                owner.thisAgent.SetDestination(owner.transform.position);

                elapsedTime += Time.deltaTime;

                if (elapsedTime >= waitTime)
                {
                    elapsedTime = 0;
                    owner.transform.Rotate(0, randomRotation * Time.deltaTime, 0);
                }

                if (owner.canSeeEnemy && owner.enemyObject != null)
                {
                    owner.botMachine.ChangeState(ChaseState.instance);
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
