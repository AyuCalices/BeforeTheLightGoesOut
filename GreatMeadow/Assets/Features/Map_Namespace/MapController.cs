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
    
    private void DrawTiles()
    {
        for (int y = 0; y < height.intValue; y++)
        {
            for (int x = 0; x < width.intValue; x++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                mapTile.InstantiateTileAt(gridPosition, mapCanv);
            }
        }
    }
    
    private void Start()
    {
        DrawTiles();

    }

    
    void Update()
    {

    }
    

    private void OnDisable()
    {

    }

}