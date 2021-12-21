using Features.Character_Namespace.Scripts;
using Features.Maze_Namespace.Scripts;

namespace Features.Interactable_Namespace.Scripts
{
    public abstract class InteractableBehaviour : GameObjectActiveSwitchBehaviour
    {
        public abstract void Interact(PlayerControllerBehaviour playerController);

        public virtual bool CanBeInteracted() => true;
    }
}
