using UnityEngine;
using UnityEngine.InputSystem;
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
        private InputAction sprinting; //TODO remove?
        private bool isRunning;
        private Vector2 smoothInputMovement;
        private Animator animator;
        private Vector2 inputMovement;
        private static readonly int HorizontalMovement = Animator.StringToHash("Horizontal");
        private static readonly int VerticalMovement = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");


        private void Awake()
        {
            transform.position = playerPosition.GetVariableValue();
            playerInputActions = new PlayerInputActions();
            playerInputActions.Enable();
            animator = GetComponent<Animator>();

            //walk
            playerInputActions.Player.Movement.performed += OnMovement;
            playerInputActions.Player.Movement.started += OnMovement;
            playerInputActions.Player.Movement.canceled += OnMovement;
            
            //run
            playerInputActions.Player.Run.performed += OnRun;
            playerInputActions.Player.Run.started += OnRun;
            playerInputActions.Player.Run.canceled += OnRun;
        }

        private void OnEnable()
        {
            //Move
            movement = playerInputActions.Player.Movement;
            movement.Enable();

            //Run
            sprinting = playerInputActions.Player.Run;
            sprinting.Enable();
            
            //Pick Up
            playerInputActions.Player.PickUp.performed += PickItUp;
            playerInputActions.Player.PickUp.Enable();
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
            SetAnimationMovement();
            
            //TODO: Set animation to sprinting
            if (isRunning)
            {
                speed = 0.8f;
            }
            else
            {
                Debug.Log("isRunning is false 0.4");
                speed = 0.4f;
                isRunning = false;
                
            }
           
                //Timer timer = new Timer()
                //timer.start
                //if (timer = 0 or sprinting.released) { speed = 0.4 }
                //if(sprinting.triggered) {timer.start}
               
                //TODO: pick up item to boost timer time
        }

        //Unused method
        private void OnDisable()
        {
            movement.Disable();
            playerInputActions.Player.PickUp.Disable();
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
            storedInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            isRunning = true;
        }

        private Vector2 GetInputMovement()
        {
            return inputMovement;
        }
        
       

        private void SetAnimationMovement()
        {
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
        
    
        /**
        * Method for picking up items
        */
        private void PickItUp(InputAction.CallbackContext obj)
        {
            Debug.Log("Pick Up not implemented yet");
            //LeanTween.moveLocal(gameObject, new Vector2(0, 0),5f);
            //Add life to health bar when picking up items
        }
    }
}
