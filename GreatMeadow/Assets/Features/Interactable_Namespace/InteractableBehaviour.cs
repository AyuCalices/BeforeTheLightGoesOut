using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace;
using UnityEngine;

public abstract class InteractableBehaviour : MonoBehaviour
{
    public abstract void Interact(PlayerController2D playerController);
}
