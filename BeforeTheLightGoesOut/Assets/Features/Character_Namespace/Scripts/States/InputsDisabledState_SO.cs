using Features.GameStates_Namespace.Scripts;
using UnityEngine;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "MovementDisabledState", menuName = "CharacterStates/MovementDisabled")]
    public class InputsDisabledState_SO : State_SO
    {
        [SerializeField] private CharacterStateController_SO characterStateController;
        
        private AudioSource WalkSounds => characterStateController.AudioSource;
        private PlayerInputActions PlayerInputActions => characterStateController.PlayerInputActions;
        
        public override void Enter()
        {
            PlayerInputActions.Disable();
            WalkSounds.enabled = false;
        }
    }
}
