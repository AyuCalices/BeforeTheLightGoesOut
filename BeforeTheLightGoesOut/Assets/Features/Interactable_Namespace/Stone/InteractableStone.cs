using System;
using System.Collections;
using DataStructures.Variables;
using Features.Character_Namespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class InteractableStone : InteractableBehaviour
{
    [SerializeField] private BoolVariable playerIsKillable;
    [SerializeField] private ExploderFocus exploderFocus;
    [SerializeField] private SpriteExploderWithoutPhysics stoneExploderPrefab;
    
    private Animator animator;
    private PlayerController2D playerController;
    
    private SpriteRenderer spriteRenderer;
    private SpriteExploderWithoutPhysics spriteExploder;

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
        playerController.EnableWalk();
        isInteractable = true;
    }

    //used by an animator event
    public void OnPlayerIsHidden()
    {
        isInteractable = true;
        playerIsKillable.Set(false);
    }

    public override void Interact(PlayerController2D playerController)
    {
        this.playerController = playerController;

        if (!isInteractable) return;
        
        isInteractable = false;
        spriteExploder = Instantiate(stoneExploderPrefab, transform);
        exploderFocus.SetExploderFocus(spriteRenderer, spriteExploder);
        
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
            playerController.DisableWalk();
        }
    }
}
