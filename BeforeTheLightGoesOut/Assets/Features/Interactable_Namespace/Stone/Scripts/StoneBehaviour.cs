using DataStructures.Variables;
using Features.Character_Namespace.Scripts;
using Features.Interactable_Namespace.Scripts;
using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using Utils.Variables;

namespace Features.Interactable_Namespace.Stone.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class StoneBehaviour : InteractableBehaviour
    {
        [SerializeField] private BoolVariable playerIsKillable;
        [SerializeField] private ExploderFocus_SO exploderFocusSo;
        [SerializeField] private SpriteExploderBehaviour stoneExploderPrefab;
    
        private Animator animator;
        private PlayerControllerBehaviour playerController;
    
        private SpriteRenderer spriteRenderer;
        private SpriteExploderBehaviour spriteExploder;

        private bool playerIsHidden;
        private bool isInteractable;
    
        private static readonly int Hide = Animator.StringToHash("Hide");
        private static readonly int Unhide = Animator.StringToHash("Unhide");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            isInteractable = true;
            playerIsKillable.Set(true);
        }

        //used by an animator event
        public void ContinuePlayerMovement()
        {
            playerController.GetComponent<SpriteRenderer>().enabled = true;
            //playerController.EnableWalk();
            isInteractable = true;
        }

        //used by an animator event
        public void OnPlayerIsHidden()
        {
            isInteractable = true;
            playerIsKillable.Set(false);
        }

        public override void Interact(PlayerControllerBehaviour playerController)
        {
            this.playerController = playerController;

            if (!isInteractable) return;
        
            isInteractable = false;
            spriteExploder = Instantiate(stoneExploderPrefab, transform);
            exploderFocusSo.SetExploderFocus(spriteRenderer, spriteExploder);
        
            if (playerIsHidden)
            {
                animator.SetTrigger(Unhide);
                playerIsHidden = false;
                playerIsKillable.Set(true);
                Destroy(spriteExploder);
                playerController.SetAsSpriteExploder();
            }
            else
            {
                animator.SetTrigger(Hide);
                playerIsHidden = true;
                playerController.GetComponent<SpriteRenderer>().enabled = false;
                //playerController.DisableWalk();
            }
        }
    }
}
