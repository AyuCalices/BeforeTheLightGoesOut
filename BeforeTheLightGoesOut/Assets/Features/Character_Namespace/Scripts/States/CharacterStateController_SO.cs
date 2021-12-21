using Features.GameStates_Namespace.Scripts;
using UnityEngine;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "CharacterStateController", menuName = "CharacterStates/Controller")]
    public class CharacterStateController_SO : StateController_SO
    {
        public Animator Animator { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public Transform Transform { get; private set; }
        public PlayerInputActions PlayerInputActions { get; private set; }
        
        public void SetReferences(Animator animator, AudioSource audioSource, PlayerInputActions playerInputActions, Transform transform)
        {
            Animator = animator;
            AudioSource = audioSource;
            Transform = transform;
            PlayerInputActions = playerInputActions;
            
            Initialize();
        }
    }
}
