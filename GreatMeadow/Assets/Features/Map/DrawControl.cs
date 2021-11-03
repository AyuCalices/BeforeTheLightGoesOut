using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* SOURCES:
    https://www.youtube.com/watch?v=_ILOVprdq4o
    https://www.youtube.com/watch?v=YHC-6I_LSos
    https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html
    https://www.youtube.com/watch?v=FQz8_uwcIqs
    */

public class DrawControl : MonoBehaviour
{
    // the script utilizes the camera position to draw
    public Camera mapCam;

    // represents drawn lines on the map
    public GameObject pen;

    // represents erasure attempts on the map
    public GameObject rubber;
    
    // processes drawing on the map
    LineRenderer lr;

    // vector to memorize last position of mouse cursor
    Vector2 lastPos;

    private int posIndex;

    private GameObject[] list;

    private int listIndex;


    private void Start()
    {
        listIndex = 0;
    }

    void Update()
    {
        // capture & interpret mouse movement and clicking by the player
        // if (Mouse.current.press.wasReleasedThisFrame) {
        // detect mouse position


        // start drawing as soon as left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartLine();
        }

        // draw a straight line from last position to mouse position (readjustable)
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            DrawStraightLine();
        }

        // draw as long as left mouse button is held
        if (Mouse.current.leftButton.isPressed)
        {
            ExtendLine();
        }

        // start erasing as soon as right mouse button is clicked
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Erase();
        }

        
        // }
    }

    void StartLine()
    {
        Vector2 currPos = mapCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameObject line = Instantiate(pen);
       // listIndex += 1;
        
        // list[listIndex] = line
        
        // get the line renderer from the line game object
        lr = line.GetComponent<LineRenderer>();
        // draw at mouse position
        lr.SetPosition(0, currPos);
        lr.SetPosition(1, currPos);
    }

    void ExtendLine()
    {

        // load current mouse position from camera
        Vector2 mousePos = (Vector2) mapCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // if the mouse cursor has been moved
        if (mousePos != lastPos)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                // increase amount of drawing points by 1
                lr.positionCount++;
                // determine index of next point to draw
                posIndex = lr.positionCount - 1;
                // draw point at position of mouse cursor
                lr.SetPosition(posIndex, (Vector2) mousePos);
            }
            // if there is no input from the player
            else
            {
                lr = null;
            }
            // draw on mouse position
            lastPos = mousePos;
        }

    }

    void DrawStraightLine()
    {
        Vector2 mousePos = (Vector2) mapCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        posIndex = lr.positionCount - 1;
        lr.SetPosition(posIndex, mousePos);
        posIndex =- 1;
    }

    void Erase()
    {
        Destroy(list[listIndex]);
        listIndex = -1;
    }
}

/*
public Map_Input inp;
public InputAction draw;
private InputAction erase;

private void Awake()
{
    inp = new Map_Input();
}

private void OnEnable()
{
    draw = inp.drawrase.drawing;
    erase = inp.drawrase.erasing;
    draw.Enable();
    erase.Enable();

    inp.drawrase.drawing.performed += DoDraw;
    mapInput.drawrase.erasing.Enable();

}

private void OnDisable()
{
    draw.Disable();
    erase.Disable();
    inp.drawrase.drawing.Disable();
    inp.drawrase.drawing.Disable();
}

private void DoDraw(InputAction.CallbackContext obj)
{
    Debug.Log("Drawing recognized");
    // throw new NotImplementedException();
}

*/

/*
MapInput inp;
public Vector2 input_Mouse;


private void Awake()
{
    
inp = new MapInput();
inp.Enable();
inp.Mouse.Drawing.performed += x => input_Mouse = x.ReadValue<Vector2>();
}

public GameObject ui_canvas;
private GraphicRaycaster ui_raycaster;
private PointerEventData click_data;
private List<RaycastResult> click_results;

void Start()
{
    ui_raycaster = ui_canvas.GetComponent<GraphicRaycaster>();
    click_data = new PointerEventData(EventSystem.current);
    click_results = new List<RaycastResult>();
}

private void Update()
{
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        Draw();
    }
//Debug.Log(input_Mouse);
//Debug.Log(Input.mousePosition);
}

}

void Draw()
{
    click_data_position = Mouse.current.position.ReadValue();
    click_results.Clear();

    ui_raycaster.raycast(click_data_position, click_results);

    foreach (RaycastResult result in click_results)
    {
        GameObject ui_element = result.gameObject;
        
        Debug.Log(ui_element);
        Debug.Log("Bla");
    }
}


*/

