using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace;
using Features.Character_Namespace.Scripts;
using Features.Maze_Namespace.Scripts;
using UnityEngine;

public abstract class InteractableBehaviour : GameObjectActiveSwitchBehaviour
{
    public abstract void Interact(PlayerControllerBehaviour playerController);

    public virtual bool CanBeInteracted() => true;
}
