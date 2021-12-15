using Features.GameStates.Scripts;
using UnityEngine;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "MovementDisabledState", menuName = "CharacterStates/MovementDisabled")]
    public class MovementDisabledState_SO : State_SO
    {
        [SerializeField] private CharacterStateController_SO characterStateController;
        
        private PlayerInputActions PlayerInputActions => characterStateController.PlayerInputActions;
        private Animator CharacterAnimator => characterStateController.Animator;
        private Transform CharacterTransform => characterStateController.Transform;
        private AudioSource WalkSounds => characterStateController.AudioSource;
        
        public override void Enter()
        {
            WalkSounds.enabled = false;
        }

        public override void Exit()
        {
            WalkSounds.enabled = true;
        }
    }
}
