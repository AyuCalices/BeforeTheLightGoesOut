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

    [SerializeField] private Transform mapCanv;
    [SerializeField] private MiniMapTileGenerator_SO mapTile;
    [SerializeField]private GameObject child;
    
    private PlayerInputActions playerInputActions;
    private InputAction mapHandling;
    public List<GameObject> shownTiles;
    [SerializeField] private IntVariable tilePos;

    
    private void DrawTiles()
    {
        for (int y = 0; y < height.intValue; y++)
        {
            for (int x = 0; x < width.intValue; x++)
            {
                int pos = y * width.intValue + x;
                Vector2Int gridPosition = new Vector2Int(x, y);
                mapTile.InstantiateTileAt(gridPosition, mapCanv);
                shownTiles.Add(mapCanv.GetChild(pos).gameObject);
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
        if (this.child.activeSelf == true)
        {
            this.child.SetActive(false);
        }
        else
        {
            this.child.SetActive(true);
        }

    }

}