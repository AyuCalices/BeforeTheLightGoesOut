using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace;
using UnityEngine;

public abstract class InteractableBehaviour : GameObjectActiveSwitchBehaviour
{
    public abstract void Interact(PlayerController2D playerController);

    public virtual bool CanBeInteracted() => true;
}
