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
    private Vector2 direction = Vector2.zero;
    public float speed = 0.01f;
    public Rigidbody2D playerRB;
    public Vector2 storedInputMovement;
    private Vector2 smoothInputMovement;
    public float movementSmoothingSpeed = 1f;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        
        //walk
        playerInputActions.Player.Movement.performed += OnMovement;
        playerInputActions.Player.Movement.started += OnMovement;
        playerInputActions.Player.Movement.canceled += OnMovement;
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
    
    //Update Loop - Used for calculating frame-based data
    void Update()
    {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
    }

    //Input's Axes values are raw
    void CalculateMovementInputSmoothing()
    {
        
        smoothInputMovement = Vector2.Lerp(smoothInputMovement, storedInputMovement, Time.deltaTime * movementSmoothingSpeed);

    }

    void UpdatePlayerMovement()
    {
        Vector2 movement = smoothInputMovement * speed * Time.deltaTime;
        Vector2 playerPosition = transform.position;
        playerPosition += movement;
        transform.position = playerPosition;
    }

    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.PickUp.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        storedInputMovement = new Vector2(inputMovement.x, inputMovement.y);
    }
    
    /**
     * Method for item pick up
     */
    public void PickItUp(InputAction.CallbackContext obj)
    {
        Debug.Log("PICK UP");
        //Add life to health bar
        LeanTween.moveLocal(gameObject, new Vector2(0, 0),5f);
        //current position > determine which direction she can move to
        //change position from tile to tile
        
    }

}
