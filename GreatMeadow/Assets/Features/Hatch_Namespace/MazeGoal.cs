using System;
using System.Collections;
using System.Collections.Generic;
using Features.Character_Namespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Variables_Namespace;

public class MazeGoal : InteractableBehaviour
{
    [SerializeField] private Vector2Variable hatchPosition;
    [SerializeField] private Vector2Variable hatchSpawnPos;
    [SerializeField] private Vector2Variable playerSpawnPos;
    [SerializeField] private IntVariable width;
    [SerializeField] private IntVariable height;
    private Animator animator;
    private static readonly int JumpInHatch = Animator.StringToHash("JumpInHatch");


    /**
    * Set the Hatch Position at the opposite of the starting position.
    */
    public void SetHatchPosition() {
        int startX = (int) playerSpawnPos.vec2Value.x;
        int startY = (int) playerSpawnPos.vec2Value.y;
           
        int endX = width.intValue - startX; 
        int endY = height.intValue - startY;

        hatchSpawnPos.vec2Value = playerSpawnPos.vec2Value;
        //hatchSpawnPos.vec2Value = new Vector2(endX, endY);
        Debug.Log("hatch pos variable value: " + hatchSpawnPos.GetVariableValue());
        transform.position = hatchPosition.vec2Value;
    }

    public override void Interact(PlayerController2D playerController)
    {
        Debug.Log("Jump in hatch");
        animator = GetComponent<Animator>();
        animator.SetTrigger(JumpInHatch);
        playerController.GetComponent<SpriteRenderer>().enabled = false; //remove character sprite
        //fackel radius kleiner
        playerController.GetComponentInChildren<Light2D>().pointLightOuterRadius -= 0.1f * Time.deltaTime;
        
        
        //show win screen
        //maybe play sound
    }
}
