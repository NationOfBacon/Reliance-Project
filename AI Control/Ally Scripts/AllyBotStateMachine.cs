using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyAIMachineTools
{
    public class AllyBotStateMachine : MonoBehaviour
    {
        public AllyState currentState;
        public string stateName;
        public AIAllyMachine owner;

        public void Start()
        {
            owner = GetComponent<AIAllyMachine>();
            currentState = AllyIdleState.instance;
            stateName = currentState.ToString();
            ChangeState(AllyIdleState.instance);
        }

        public void ChangeState(AllyState _newState)
        {
            currentState = _newState;
            Debug.Log(owner.displayName + ": has entered " + currentState);
            stateName = currentState.ToString();

            if(owner.gameObject.activeSelf)
                StartCoroutine(currentState.InState(owner));
        }

    }

    public abstract class AllyState
    {
        public abstract IEnumerator InState(AIAllyMachine owner);
    }
}
