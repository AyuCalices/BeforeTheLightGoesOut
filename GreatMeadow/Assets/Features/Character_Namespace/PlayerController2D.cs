using DataStructures.Variables;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Event_Namespace;

namespace Features.Character_Namespace
{
    public class PlayerController2D : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Vector2IntVariable playerIntPosition;
        [SerializeField] private Vector2Variable playerFloatPosition;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameEvent onLoadLoseMenu;
        [SerializeField] private SpriteExploderWithoutPhysics spriteExploder;
        
        [Header("Balancing")]
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private float movementSmoothingSpeed = 1f;
        
        [Header("Events")]
        [SerializeField] private GameEvent onInteractableTriggerEnter;
        [SerializeField] private GameEvent onInteractableTriggerExit;
        

        private InteractableBehaviour currentInteractable;
        private bool playerIsDead;
        
        //movement
        private PlayerInputActions playerInputActions;
        private InputAction movement;
        private Vector2 smoothInputMovement;
        private Vector2 inputMovement;
        private Vector2 storedInputMovement;

        //animator
        private Animator animator;
        private static readonly int HorizontalMovement = Animator.StringToHash("Horizontal");
        private static readonly int VerticalMovement = Animator.StringToHash("Vertical");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
        
        
        public void InitializePlayer()
        {
            transform.position = (Vector2)playerIntPosition.Get();
        }

        public void TriggerDeath()
        {
            playerIsDead = true;
            spriteExploder.ExplodeSprite();
            GetComponent<SpriteRenderer>().enabled = false;
            onLoadLoseMenu.Raise();
        }

        private void Awake()
        {
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
        }
        
        private void OnDisable()
        {
            movement.Disable();
            playerInputActions.Player.Interact.Disable();
        }
    
        //Update Loop - Used for calculating frame-based data
        private void Update()
        {
            if (playerIsDead) return;
            
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();

            var position = transform.position;
            playerIntPosition.Set(new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)));
            playerFloatPosition.Set(position);
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
            animator.SetFloat(HorizontalMovement, inputMovement.x);
            animator.SetFloat(VerticalMovement, inputMovement.y);
            GetComponent<AudioSource>().Pause();

            //Get into idle position
            if (inputMovement.x != 0  || inputMovement.y != 0)
            {
                animator.SetFloat(LastMoveX, inputMovement.x);
                animator.SetFloat(LastMoveY, inputMovement.y);
                GetComponent<AudioSource>().UnPause();
            }
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
            storedInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        }
        
        //Trigger event when player gets near the interactable object.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            currentInteractable = collider.GetComponent<InteractableBehaviour>();
            if (currentInteractable != null)
            {
                onInteractableTriggerEnter.Raise();
                playerInputActions.Player.Interact.performed += OnPerformInteraction;
            }
        }
        
        //Triggers event when player moves away from the interactable object.
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (currentInteractable != null)
            {
                playerInputActions.Player.Interact.performed -= OnPerformInteraction;
                onInteractableTriggerExit.Raise();
                currentInteractable = null;
            }
        }
        
        //If E is pressed, player interacts with object.
        private void OnPerformInteraction(InputAction.CallbackContext context)
        {
            currentInteractable.Interact(this);
        }
        
        public void OnPerformInteraction()
        {
            currentInteractable.Interact(this);
        }
    }
}
