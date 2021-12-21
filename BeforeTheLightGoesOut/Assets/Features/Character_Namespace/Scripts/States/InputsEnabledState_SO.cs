using System.Linq;
using Features.GameStates_Namespace.Scripts;
using UnityEngine;
using Utils.Variables;
using UnityEngine.InputSystem;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "MovementEnabledState", menuName = "CharacterStates/MovementEnabled")]
    public class InputsEnabledState_SO : State_SO
    {
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private Vector2Variable playerFloatPosition;
        [SerializeField] private Vector2IntVariable playerIntPosition;
        [Header("Balancing")]
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private float movementSmoothingSpeed = 1f;
        
        private Vector2 _smoothInputMovement;
        private Vector2 _storedInputMovement;
        private Vector2 _inputMovement;

        private PlayerInputActions PlayerInputActions => characterStateController.PlayerInputActions;
        private Animator CharacterAnimator => characterStateController.Animator;
        private Transform CharacterTransform => characterStateController.Transform;
        private AudioSource WalkSounds => characterStateController.AudioSource;
        
        private static readonly int HorizontalMovement = Animator.StringToHash("Horizontal");
        private static readonly int VerticalMovement = Animator.StringToHash("Vertical");
        private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
        private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
        
        public override void Enter()
        {
            PlayerInputActions.Enable();
            PlayerInputActions.Player.Movement.performed += OnMovement;
            PlayerInputActions.Player.Movement.started += OnMovement;
            PlayerInputActions.Player.Movement.canceled += OnMovement;
            
            WalkSounds.enabled = true;
        }

        public override void Execute()
        {
            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();

            var position = CharacterTransform.position;
            playerIntPosition.Set(new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)));
            playerFloatPosition.Set(position);
        }

        public override void Exit()
        {
            PlayerInputActions.Player.Movement.performed -= OnMovement;
            PlayerInputActions.Player.Movement.started -= OnMovement;
            PlayerInputActions.Player.Movement.canceled -= OnMovement;
        }
        
        //Input's Axes values are raw
        private void CalculateMovementInputSmoothing()
        {
            _smoothInputMovement = Vector2.Lerp(_smoothInputMovement, _storedInputMovement, Time.deltaTime * movementSmoothingSpeed);
        }
        
        private void OnMovement(InputAction.CallbackContext context)
        {
            _inputMovement = context.ReadValue<Vector2>();
            _storedInputMovement = new Vector2(_inputMovement.x, _inputMovement.y);
        }

        private void UpdatePlayerMovement()
        {
            Vector2 movement = _smoothInputMovement * speed * Time.deltaTime;
            Vector2 playerPosition = CharacterTransform.position;
            playerPosition += movement;
            CharacterTransform.position = playerPosition;
        
            //Set animation to movement
            CharacterAnimator.SetFloat(HorizontalMovement, _inputMovement.x);
            CharacterAnimator.SetFloat(VerticalMovement, _inputMovement.y);
            WalkSounds.Pause();

            //Get into idle position
            if (_inputMovement.x != 0  || _inputMovement.y != 0)
            {
                CharacterAnimator.SetFloat(LastMoveX, _inputMovement.x);
                CharacterAnimator.SetFloat(LastMoveY, _inputMovement.y);
                WalkSounds.UnPause();
            }
        }
    }
}
