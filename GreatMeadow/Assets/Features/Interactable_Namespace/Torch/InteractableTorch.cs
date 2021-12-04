using Features.Character_Namespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Variables_Namespace;

[RequireComponent(typeof(Animator))]
public class InteractableTorch : InteractableBehaviour
{
    [SerializeField] private Vector2Variable torchSpawnPos;
    [SerializeField] private Vector2Variable torchPosition;
    [SerializeField] private Light2D torchLight;
    private Animator animator;
    private static readonly int PickUpTorch = Animator.StringToHash("PickUpTorch");

    /**
    * Set spawn position of torch
    */
    public void SetTorchPosition(Vector2Variable torchSpawnPosition)
    {
        torchSpawnPos = torchSpawnPosition;
        transform.position = torchPosition.vec2Value;
    }

    /**
     * Pick Up Torch
     */
    public override void Interact(PlayerController2D playerController)
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(PickUpTorch);
        torchLight.intensity = 0f;
        playerController.GetComponentInChildren<PlayerTorch>().RefillTorch();
    }
}
