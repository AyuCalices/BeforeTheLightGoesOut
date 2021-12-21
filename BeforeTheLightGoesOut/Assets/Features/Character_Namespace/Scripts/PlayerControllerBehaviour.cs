using System;
using Features.GameStates.Character;
using Features.Interactable_Namespace.Scripts;
using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Event_Namespace;
using Utils.Variables;

namespace Features.Character_Namespace.Scripts
{
    public class PlayerControllerBehaviour : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private InputsEnabledState_SO inputsEnabledState;
        [SerializeField] private Vector2IntVariable playerIntPosition;
        
        [SerializeField] private SpriteExploderBehaviour spriteExploder;
        [SerializeField] private ExploderFocus_SO exploderFocus;
        
        [SerializeField] private GameEvent onInteractableTriggerEnter;
        [SerializeField] private GameEvent onInteractableTriggerExit;
        [SerializeField] private AudioSource walkSound;
        

        private PlayerInputActions playerInputActions;
        private InteractableBehaviour currentInteractable;
        private bool playerCanWalk;

        //animator
        private SpriteRenderer spriteRenderer;
        private Animator animator;


        public void InitializePlayer()
        {
            transform.position = (Vector2)playerIntPosition.Get();
        }
        
        public void SetAsSpriteExploder()
        {
            exploderFocus.SetExploderFocus(spriteRenderer, spriteExploder);
        }
        
        public void PerformInteraction()
        {
            currentInteractable.Interact(this);
        }
        
        //used by an animator event
        public void RequestMovementEnabledState()
        {
            characterStateController.RequestState(inputsEnabledState);
        }

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
            
            characterStateController.SetReferences(GetComponent<Animator>(), walkSound, playerInputActions, transform);
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetAsSpriteExploder();
        }

        private void Update()
        {
            characterStateController.UpdateState();
        }

        //Trigger event when player gets near the interactable object.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            currentInteractable = collider.GetComponent<InteractableBehaviour>();
            if (currentInteractable != null && currentInteractable.CanBeInteracted())
            {
                onInteractableTriggerEnter.Raise();
                playerInputActions.Player.Interact.Enable();
                playerInputActions.Player.Interact.performed += OnPerformInteraction;
            }
        }
        
        //Triggers event when player moves away from the interactable object.
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (currentInteractable != null)
            {
                onInteractableTriggerExit.Raise();
                playerInputActions.Player.Interact.Disable();
                playerInputActions.Player.Interact.performed -= OnPerformInteraction;
                currentInteractable = null;
            }
        }
        
        //If E is pressed, player interacts with object.
        private void OnPerformInteraction(InputAction.CallbackContext context)
        {
            currentInteractable.Interact(this);
        }
    }
}
