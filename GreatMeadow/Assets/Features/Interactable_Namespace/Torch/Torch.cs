using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Event_Namespace;
using Utils.Variables_Namespace;

public class Torch : InteractableBehaviour
{
    [SerializeField] private Vector2Variable playerSpawnPos;
    [SerializeField] private Vector2Variable torchSpawnPos;
    [SerializeField] private Vector2Variable torchPosition;
    [SerializeField] private IntVariable width;
    [SerializeField] private IntVariable height;
    [SerializeField] private GameEvent onLoadLoseMenu;
    private Animator animator;
    private static readonly int PickUpTorch = Animator.StringToHash("PickUpTorch");
    private float currentTorchDurability = 1.0f;
    private float maxTorchDurability = 1f;
    private float highestTorchBrightness = 1.3f;
    private float lowestTorchBrighness = 0.66f;
    private float torchBrightness = 1.3f;

    private void Update()
    {
        
        //currentTorchDurability -= Time.deltaTime;
        //Debug.Log("currentTorchDurab: " + currentTorchDurability);
        //float torchDurabilityInPercent = currentTorchDurability / maxTorchDurability;
        //Debug.Log("torchDurabilityInPercent: " + torchDurabilityInPercent);
        //torchBrightness = Mathf.Max(lowestTorchBrighness, torchDurabilityInPercent * highestTorchBrightness);
        //Debug.Log("torchBrightness: " + torchBrightness);
        //GetComponentInChildren<Light2D>().intensity = torchBrightness;
        //Debug.Log("intensity: " + GetComponentInChildren<Light2D>().intensity);
    }

    /**
    * Set spawn position of torch
    */
    public void SetTorchPosition()
    {
        torchSpawnPos.vec2Value = playerSpawnPos.vec2Value; //for testing
        Debug.Log("Torch spawn pos: " + torchSpawnPos.vec2Value);
        transform.position = torchPosition.vec2Value;
    }

    /**
     * Pick Up Torch
     */
    public override void Interact(PlayerController2D playerController)
    {
        Debug.Log("PICk up");
        animator = GetComponent<Animator>();
        animator.SetTrigger(PickUpTorch);
        //playerController.GetComponent<SpriteRenderer>().enabled = false;
    }
    
    /**
     * If torch burns down
     */
    public void LoadLoseMenu()
    {
        onLoadLoseMenu.Raise();
    }
}
