using DataStructures.Variables;
using Features.Character_Namespace.Scripts;
using Features.GameStates;
using Features.GameStates.Character;
using Features.GameStates_Namespace.Scripts.States;
using Features.Interactable_Namespace.Scripts;
using Features.Maze_Namespace.Scripts;
using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using Utils.Event_Namespace;
using Utils.Variables;

namespace Features.Interactable_Namespace.Hatch.Scripts
{
    public class HatchBehaviour : InteractableBehaviour
    {
        [Header("States")]
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private InputsDisabledState_SO inputsDisabledState;
        [SerializeField] private GameStateController_SO gameStateController;
        
        [Header("Asset References")]
        [SerializeField] private TileList_SO tileList;
        [SerializeField] private Vector2IntVariable hatchPosition;
        [SerializeField] private GameEvent onLoadWinMenu;
        [SerializeField] private ExploderFocus_SO exploderFocusSo;
        
        [Header("Scene References")]
        [SerializeField] private SpriteExploderBehaviour hatchExploder;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool isInteracted;
        private Animator animator;
        private static readonly int JumpInHatch = Animator.StringToHash("JumpInHatch");

        public void Initialize()
        {
            transform.position = (Vector2)hatchPosition.Get();
            tileList.GetTileAt(hatchPosition.Get()).RegisterInteractable(this);
        }

        //used by an animator event
        public void LoadWinMenu()
        {
            if (gameStateController.GetState() is PlayState_SO)
            {
                onLoadWinMenu.Raise();
            }
        }

        public override void Interact(PlayerControllerBehaviour playerController)
        {
            if (isInteracted) return;
            isInteracted = true;
        
            GetComponent<AudioSource>().Play();
            animator = GetComponent<Animator>();
            animator.SetTrigger(JumpInHatch);
            playerController.GetComponent<SpriteRenderer>().enabled = false;
            characterStateController.RequestState(inputsDisabledState);
        
            exploderFocusSo.SetExploderFocus(spriteRenderer, hatchExploder);
        }
    }
}
