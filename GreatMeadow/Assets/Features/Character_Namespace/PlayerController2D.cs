using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Event_Namespace;
using Utils.Variables_Namespace;

namespace Features.Character_Namespace
{
    public class PlayerController2D : MonoBehaviour
    {
        [SerializeField] private Vector2Variable playerPosition;
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private Vector2 storedInputMovement;
        [SerializeField] private float movementSmoothingSpeed = 1f;
        private PlayerInputActions playerInputActions;
        private InputAction movement;
        private Vector2 smoothInputMovement;
        private Animator animator;
        private Vector2 inputMovement;
        private static readonly int HorizontalMovement = Animator.StringToHash("Horizontal");
        private static readonly int VerticalMovement = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
        private InteractableBehaviour currentInteractable;
        
        public void SetPlayerPosition()
        {
            transform.position = playerPosition.GetVariableValue();
        }

        private void Awake()
        {
            //Debug.Log("player pos variable value: " + playerPosition.GetVariableValue());
            playerInputActions = new PlayerInputActions();
            playerInputActions.Enable();
            animator = GetComponent<Animator>();

            //walk
            playerInputActions.Player.Movement.performed += OnMovement;
            playerInputActions.Player.Movement.started += OnMovement;
            playerInputActions.Player.Movement.canceled += OnMovement;
        }

        private void OnEnable()
        {
            //Move
            movement = playerInputActions.Player.Movement;
            movement.Enable();
        
            //Pick Up
            //playerInputActions.Player.Interact.performed += Interact;
            //playerInputActions.Player.Interact.Enable();
        }
    
        //Update Loop - Used for calculating frame-based data
        private void Update()
        {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
        }

        //Input's Axes values are raw
        private void CalculateMovementInputSmoothing()
        {
            smoothInputMovement = Vector2.Lerp(smoothInputMovement, storedInputMovement, Time.deltaTime * movementSmoothingSpeed);
        }

        private void UpdatePlayerMovement()
        {
            Vector2 movement = smoothInputMovement * speed * Time.deltaTime;
            Vector2 playerPosition = transform.position;
            playerPosition += movement;
            transform.position = playerPosition;
        
            //Set animation to movement
            animator.SetFloat(HorizontalMovement, GetInputMovement().x);
            animator.SetFloat(VerticalMovement, GetInputMovement().y);
            animator.SetFloat(Speed, GetInputMovement().sqrMagnitude);
            //Get into idle position
            if (GetInputMovement().x == 1 || GetInputMovement().x == -1 || GetInputMovement().y == 1 || GetInputMovement().y == -1)
            {
                animator.SetFloat(LastMoveX, GetInputMovement().x);
                animator.SetFloat(LastMoveY, GetInputMovement().y);
            }
        }

        private void OnDisable()
        {
            movement.Disable();
            playerInputActions.Player.Interact.Disable();
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
            storedInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        }

        private Vector2 GetInputMovement()
        {
            return inputMovement;
        }
        
        /**
         * Trigger event when player gets near the hatch.
         */
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            currentInteractable = collider2D.GetComponent<InteractableBehaviour>();
            if (currentInteractable != null)
            {
                Debug.Log(collider2D.gameObject.name + " Enter");
                playerInputActions.Player.Interact.performed += OnPerformInteraction;
            }
        }
        
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (currentInteractable != null)
            {
                Debug.Log(collider.gameObject.name + " Exit");
                playerInputActions.Player.Interact.performed -= OnPerformInteraction;
                currentInteractable = null;
            }
        }
        /**
         * If E is pressed, player interacts with object.
         */
        public void OnPerformInteraction(InputAction.CallbackContext context)
        {
            currentInteractable.Interact(this);
        }
        
        private void Interact(InputAction.CallbackContext obj)
        {
            Debug.Log("Interact not implemented yet");
        }
    }
}
