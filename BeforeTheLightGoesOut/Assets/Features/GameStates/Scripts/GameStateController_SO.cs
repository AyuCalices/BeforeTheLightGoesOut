using Features.Music_Namespace.Scripts;
using UnityEngine;
using Utils.CanvasNavigator;
using Utils.StateMachine_Namespace;

namespace Features.GameStates.Scripts
{
    [CreateAssetMenu(fileName = "StateController", menuName = "GameStates/StateController")]
    public class GameStateController_SO : ScriptableObject
    {
        [SerializeField] private State_SO startingState;
        private StateMachine stateMachine;
        
        public CanvasManager CanvasManager { get; private set; }
        public CanvasGroup FadeMenu { get; private set; }
        public MusicBehaviour MusicBehaviour { get; private set; }

        public void Initialize(CanvasManager canvasManager, CanvasGroup fadeMenu, MusicBehaviour musicBehaviour)
        {
            CanvasManager = canvasManager;
            FadeMenu = fadeMenu;
            MusicBehaviour = musicBehaviour;

            stateMachine = new StateMachine();
            stateMachine.Initialize(startingState);
        }

        public void RequestState(State_SO state)
        {
            if (((State_SO) GetState()).IsValidStateShift(state))
            {
                stateMachine.ChangeState(state);
            }
        }

        public IState GetState()
        {
            return stateMachine.GetCurrentState();
        }
    }
}
