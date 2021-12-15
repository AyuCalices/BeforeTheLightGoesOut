using System.Collections.Generic;
using UnityEngine;
using Utils.StateMachine_Namespace;

namespace Features.GameStates.Scripts
{
    public abstract class State_SO : ScriptableObject, IState
    {
        [field: SerializeField] protected List<State_SO> validStateShifts;
        [field: SerializeField] protected GameStateController_SO gameStateController;

        public bool IsValidStateShift(State_SO requestedState)
        {
            return validStateShifts.Contains(requestedState);
        }
        
        public abstract void Enter();

        public virtual void Execute()
        {
            Debug.Log("Update on State: " + name);
        }

        public virtual void Exit()
        {
            Debug.Log("Exit on State: " + name);
        }
    }
}
