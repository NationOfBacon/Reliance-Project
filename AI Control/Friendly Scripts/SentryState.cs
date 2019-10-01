using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineTools;

public class SentryState : State
{
    private static SentryState _instance; //only declared once

    private SentryState() //set instance to itself
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static SentryState instance //creates the instance of the first state if it does not exist
    {
        get
        {
            if (_instance == null)
            {
                new SentryState();
            }

            return _instance;
        }
    }

    public override IEnumerator InState(AIMachine owner)
    {
        float waitTime = 2f;
        float elapsedTime = 0f;

        while (owner.botMachine.currentState == SentryState.instance)
        {
            if (!owner.disabled)
            {
                owner.thisAgent.SetDestination(owner.transform.position);

                elapsedTime += Time.deltaTime;

                if (elapsedTime >= waitTime)
                {
                    //once time is greater than wait time the bot is set into sentry 

                    if (owner.canSeeEnemy && owner.enemyObject != null)
                    {
                        owner.botMachine.ChangeState(AttackState.instance); //bot will switch into a special attack state that will return to sentry state after the target dies
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

}
