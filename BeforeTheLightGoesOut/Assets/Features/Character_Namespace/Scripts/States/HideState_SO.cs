using Features.GameStates_Namespace.Scripts;
using UnityEngine;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "HideState", menuName = "CharacterStates/Hide")]
    public class HideState_SO : State_SO
    {
        [SerializeField] private CharacterStateController_SO characterStateController;
        
        private AudioSource WalkSounds => characterStateController.AudioSource;
        private PlayerInputActions PlayerInputActions => characterStateController.PlayerInputActions;
        
        public override void Enter()
        {
            WalkSounds.enabled = false;
            PlayerInputActions.Enable();
        }
    }
}
