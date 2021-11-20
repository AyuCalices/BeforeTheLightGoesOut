using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Utils.Variables_Namespace;
using Random = UnityEngine.Random;
using Scene = UnityEditor.SearchService.Scene;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private Vector2Variable playerPosition;
    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private Vector2 direction = Vector2.zero;
    public float speed = 0.01f;
    public Rigidbody2D playerRB;
    public Vector2 storedInputMovement;
    private Vector2 smoothInputMovement;
    public float movementSmoothingSpeed = 1f;
    public Animator animator;
    private Vector2 inputMovement;


    private void Awake()
    {
        transform.position = playerPosition.GetVariableValue();
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
        //Set animation to movement
        animator.SetFloat("Time", Time.deltaTime );
        animator.SetFloat("Horizontal", getInputMovement().x);
        animator.SetFloat("Vertical", getInputMovement().y);
        //fill array with last two positions, remove. if zero gets hit, get the last number back
        
        //animator.SetFloat("DirectionHorizontal", getInputMovement().y);
        //animator.SetFloat("DirectionVertical", getInputMovement().y);
        animator.SetFloat("Speed", getInputMovement().sqrMagnitude);
        //Debug.Log(transform.position);
    }

    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.PickUp.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        storedInputMovement = new Vector2(inputMovement.x, inputMovement.y);
        Debug.Log(inputMovement);
    }

    private Vector2 getInputMovement()
    {
        return inputMovement;
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
