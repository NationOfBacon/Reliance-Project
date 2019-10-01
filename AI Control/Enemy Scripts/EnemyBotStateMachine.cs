using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAIMachineTools
{
    public class EnemyBotStateMachine : MonoBehaviour
    {
        public EnemyState currentState;
        public string stateName;
        public EnemyAIMachine owner;

        public void Start()
        {
            owner = GetComponent<EnemyAIMachine>();
            currentState = null;
        }

        private void Update()
        {
            stateName = currentState.ToString();
        }

        public void ChangeState(EnemyState _newState)
        {
            currentState = _newState;

            if (owner.gameObject.activeSelf)
                StartCoroutine(currentState.InState(owner));
        }

    }

    public abstract class EnemyState
    {
        public abstract IEnumerator InState(EnemyAIMachine owner);
    }
}
