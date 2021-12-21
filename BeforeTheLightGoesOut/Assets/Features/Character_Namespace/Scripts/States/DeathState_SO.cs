using Features.GameStates_Namespace.Scripts;
using Features.GameStates_Namespace.Scripts.States;
using Features.Simple_Sprite_Exploder_Without_Physics.Scripts;
using UnityEngine;
using Utils.Event_Namespace;

namespace Features.GameStates.Character
{
    [CreateAssetMenu(fileName = "DeathState", menuName = "CharacterStates/Death")]
    public class DeathState_SO : State_SO
    {
        [SerializeField] private CharacterStateController_SO characterStateController;
        [SerializeField] private GameStateController_SO gameStateController;
        [SerializeField] private ExploderFocus_SO exploderFocus;
        [SerializeField] private GameEvent onLoadLoseMenu;
        
        private AudioSource WalkSounds => characterStateController.AudioSource;
        private PlayerInputActions PlayerInputActions => characterStateController.PlayerInputActions;
        
        public override void Enter()
        {
            if (gameStateController.GetState() is PlayState_SO)
            {
                exploderFocus.ExplodeSprite();
                onLoadLoseMenu.Raise();
            }

            WalkSounds.enabled = false;
            PlayerInputActions.Disable();
        }
    }
}
