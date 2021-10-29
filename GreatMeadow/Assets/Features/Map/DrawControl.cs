using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawControl : MonoBehaviour
{
    // the script utilizes the camera position to draw
    public Camera mapCam;
    // represents drawn lines on the map
    public GameObject line;
    // represents drawn lines on the map
    public GameObject erasure;
    // processes drawing on the map
    LineRenderer lrDraw;
    // processes erasing on the map
    LineRenderer lrErase;
    // vector to memorize last position of mouse cursor
    Vector2 lastPos;
    
    void Update()
    {
        // capture & interpret mouse movement and clicking by the player
        Click();
    }
    
    void Click()
    {
        // start drawing as soon as left mouse button is clicked
       if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startLine();
        }

        // start erasing as soon as right mouse button is clicked
       if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            startErasure();
        }

        // draw as long as left mouse button is held
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            // load current mouse position from camera
            Vector2 mousePos = mapCam.ScreenToWorldPoint(Input.mousePosition);
            // if the mouse cursor has been moved
            if (mousePos != lastPos)
            {
                // draw on mouse position
                if (Input.GetKey(KeyCode.Mouse0)) extendLine(mousePos);
                if (Input.GetKey(KeyCode.Mouse1)) extendErasure(mousePos);
                lastPos = mousePos;
            }
        }
        // if there is no input from the player
        else
        {
            lrDraw = null;
            lrErase = null;
        }
    }
   
    void startLine()
    {
        GameObject pen = Instantiate(line);
        // get the line renderer from the pen game object
        lrDraw = pen.GetComponent<LineRenderer>();
        // load current mouse position from camera
        Vector2 mousePos = mapCam.ScreenToWorldPoint(Input.mousePosition);
        // draw at mouse position
        lrDraw.SetPosition(0, mousePos);
        lrDraw.SetPosition(1, mousePos);
    }

    void extendLine(Vector2 pointPos)
    {
        // increase amount of erasure point by 1
        lrDraw.positionCount++;
        // determine index of next point to erase
        int posIndex = lrDraw.positionCount - 1;
        // erase point at position of mouse cursor
        lrDraw.SetPosition(posIndex, pointPos);
    }

void startErasure()
    {
        GameObject rubber = Instantiate(erasure);
        // get the line renderer from the pen game object
        lrErase = rubber.GetComponent<LineRenderer>();
        // load current mouse position from camera
        Vector2 mousePos = mapCam.ScreenToWorldPoint(Input.mousePosition);
        // draw at mouse position
        // parameters of SetPosition = (index of point, location of point)
        lrErase.SetPosition(0, mousePos);
        lrErase.SetPosition(1, mousePos);
    }

    void extendErasure(Vector2 pointPos)
    {
        // increase amount of erasure point by 1
        lrErase.positionCount++;
        // determine index of next point to erase
        int posIndex = lrErase.positionCount - 1;
        // erase point at position of mouse cursor
        lrErase.SetPosition(posIndex, pointPos);
    }
}
