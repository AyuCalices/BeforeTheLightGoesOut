using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Utils.Variables_Namespace;
using Random = UnityEngine.Random;
using Scene = UnityEditor.SearchService.Scene;
using Features.Maze_Namespace.Tiles;

public class MapController : MonoBehaviour
{
    [Tooltip("Width of generated maze in number of tiles (x-axis).")]
    [SerializeField] private IntVariable width;
    [Tooltip("Height of generated maze in number of tiles (y-axis).")]
    [SerializeField] private IntVariable height;

    [Tooltip("Parent transform of all map tiles children.")]
    [SerializeField] private Transform mapParent;
    
    [Tooltip("Grants access to the generation of the map.")]
    [SerializeField] private MiniMapTileGenerator_SO mapTile;

    private PlayerInputActions playerInputActions;
    private InputAction mapHandling;
    
    // list to access game object (de-)activation as part of map progression
    private List<GameObject> shownTiles;
    
    // access to the player positions' tile number
    [SerializeField] private IntVariable tilePos;

    
    private void DrawTiles()
    {
        for (int y = 0; y < height.intValue; y++)
        {
            for (int x = 0; x < width.intValue; x++)
            {
                // store position to access child index later
                int pos = y * width.intValue + x;
                
                //
                Vector2Int gridPosition = new Vector2Int(x, y);
                
                // instantiate map tile at fitting position as child of the map canvas
                mapTile.InstantiateTileAt(gridPosition, mapParent);
                
                // add the
                shownTiles.Add(mapParent.GetChild(pos).gameObject);
            }
        }
    }
    
    private void Start()
    {
        shownTiles = new List<GameObject>();

        DrawTiles();

        for (int n = 0; n < width.intValue*height.intValue; n++)
        {
        shownTiles[n].SetActive(false);
        }
    }

    private void Update()
    {
        shownTiles[tilePos.intValue].SetActive(true);
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        
    }

    private void OnEnable()
    {
        // open map
        playerInputActions.Player.OpenMap.performed += mapActivated;
        playerInputActions.Player.OpenMap.Enable();
    }

    public void mapActivated(InputAction.CallbackContext obj)
    {
        if (mapParent.gameObject.activeSelf == true)
        {
            mapParent.gameObject.SetActive(false);
        }
        else
        {
            mapParent.gameObject.SetActive(true);
        }

    }

}