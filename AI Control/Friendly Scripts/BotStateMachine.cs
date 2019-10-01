using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIMachineTools
{
    public class BotStateMachine : MonoBehaviour
    {
        public State currentState;
        public State baseState; //the state that is the basis for actions within a set of states
        public string baseStateName;
        public string currentStateName;
        public AIMachine owner;

        public void Start()
        {
            owner = GetComponent<AIMachine>();
            currentState = IdleState.instance;
            currentStateName = currentState.ToString();
            ChangeState(IdleState.instance);
        }

        public void ChangeState(State _newState)
        {
            currentState = _newState;
            Debug.Log(owner.displayName + ": has entered " + currentState);

            if (currentState == SearchState.instance)
                baseState = SearchState.instance;
            else if (currentState == CaptureAreaState.instance)
                baseState = CaptureAreaState.instance;
            else if (currentState == FollowState.instance)
                baseState = FollowState.instance;
            else if (currentState == HealState.instance)
                baseState = HealState.instance;
            else if (currentState == PartnerState.instance)
                baseState = PartnerState.instance;
            else if (currentState == SentryState.instance)
                baseState = SentryState.instance;
            else if (currentState == SupportState.instance)
                baseState = SupportState.instance;
            else if (currentState == MoveToState.instance)
                baseState = MoveToState.instance;
            else if (currentState == IdleState.instance)
                baseState = SearchState.instance;

            baseStateName = baseState.ToString();
            currentStateName = currentState.ToString();

            if (owner.gameObject.activeSelf)
                StartCoroutine(currentState.InState(owner));
        }
    }

    public abstract class State
    {
        public abstract IEnumerator InState(AIMachine owner);
    }
}

