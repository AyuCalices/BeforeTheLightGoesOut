using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController2D : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction movement;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        //Move
        movement = playerInputActions.Player.Movement;
        movement.Enable();
        
        //Pick Up
        playerInputActions.Player.PickUp.performed += PickItUp;
        playerInputActions.Player.PickUp.Enable();

        //Hide
        //playerInputActions.Player.Hide.performed += GoHide;
        //playerInputActions.Player.Hide.Enable();

    }

    /**
     * Method for item pick up
     */
    public void PickItUp(InputAction.CallbackContext obj)
    {
        Debug.Log("PICK UP");
        //Add life to health bar
        LeanTween.moveLocal(gameObject, new Vector2(0, -0.5f),5f);
        //current position > determine which direction she can move to
        //change position from tile to tile
        
    }
    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.PickUp.Disable();
    }

    

}
