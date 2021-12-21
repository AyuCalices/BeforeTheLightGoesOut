using Features.Character_Namespace.Scripts;
using Features.GameStates.Character;
using Features.Interactable_Namespace.Scripts;
using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using Utils.Variables;

namespace Features.Interactable_Namespace.Stone.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class StoneBehaviour : InteractableBehaviour
    {
        [Header("Character State")]
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private HideState_SO hideState;
        [SerializeField] private InputsEnabledState_SO inputsEnabledState;
        [SerializeField] private InputsDisabledState_SO inputsDisabledState;
        
        [Header("Asset References")]
        [SerializeField] private ExploderFocus_SO exploderFocusSo;
        [SerializeField] private SpriteExploderBehaviour stoneExploderPrefab;
    
        private Animator animator;
        private PlayerControllerBehaviour playerController;
    
        private SpriteRenderer spriteRenderer;
        private SpriteExploderBehaviour spriteExploder;

        private bool isInteractable;
    
        private static readonly int Hide = Animator.StringToHash("Hide");
        private static readonly int Unhide = Animator.StringToHash("Unhide");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            isInteractable = true;
        }

        //used by an animator event
        public void ContinuePlayerMovement()
        {
            playerController.GetComponent<SpriteRenderer>().enabled = true;

            Destroy(spriteExploder);
            playerController.SetAsSpriteExploder();
            
            characterStateController.RequestState(inputsEnabledState);
            isInteractable = true;
        }

        //used by an animator event
        public void OnPlayerIsHidden()
        {
            characterStateController.RequestState(hideState);
            isInteractable = true;
        }

        public override void Interact(PlayerControllerBehaviour playerController)
        {
            if (!isInteractable) return;
            isInteractable = false;
            
            this.playerController = playerController;
            
            if (characterStateController.GetState() is HideState_SO)
            {
                animator.SetTrigger(Unhide);
            }
            else if (characterStateController.GetState() is InputsEnabledState_SO)
            {
                animator.SetTrigger(Hide);
                
                spriteExploder = Instantiate(stoneExploderPrefab, transform);
                exploderFocusSo.SetExploderFocus(spriteRenderer, spriteExploder);

                playerController.GetComponent<SpriteRenderer>().enabled = false;
            }
            
            characterStateController.RequestState(inputsDisabledState);
        }
    }
}
