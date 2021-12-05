using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Variables_Namespace;
using Features.Maze_Namespace.Tiles;

//TODO: remove magic numbers
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

    // grants access to player input
    private PlayerInputActions playerInputActions;
    
    // handles input actions on the map scene
    private InputAction mapHandling;
    
    // list to access game object (de-)activation as part of map progression
    private List<GameObject> shownTiles;
    
    // access to the player positions' tile number
    [SerializeField] private IntVariable tilePos;

    private bool isInitialized;
    
    private void Awake()
    {
        // initialize input actions for map scene
        playerInputActions = new PlayerInputActions();
        
        // enable player input in map scene
        playerInputActions.Enable();
    }
    
    private void OnEnable()
    {
        // allow opening of the map
        playerInputActions.Player.OpenMap.performed += mapActivated;
        playerInputActions.Player.OpenMap.Enable();
    }
    
    public void InitializeMap()
    {
        // list where the map tiles will be stored
        shownTiles = new List<GameObject>();
        
        // centralize the map
        mapParent.transform.position += new Vector3(-width.intValue*5, -height.intValue*5,  0);
        
        // draw map tiles from the same maze seed
        DrawTiles();

        // loop through all map tiles and hide them at game start
        for (int n = 0; n < width.intValue*height.intValue; n++)
        {
            shownTiles[n].SetActive(false);
        }

        //TODO: use canvas for positioning line 53 & 56
        // position map background art
        mapParent.GetChild(0).position = new Vector3(Screen.width/2, Screen.height/2, 0);
        
        // position map text
        mapParent.GetChild(1).position = new Vector3(Screen.width/2, Screen.height/2+height.intValue*6, 0);
        
        // depict player position marker above all tiles
        mapParent.GetChild(2).SetAsLastSibling();

        // only start update, when initialization is completed
        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized) 
        {
            // reveal map tile at player position
            shownTiles[tilePos.intValue].SetActive(true);
            
            // depict player position on current map tile
            mapParent.GetChild(shownTiles.Count+2).position = shownTiles[tilePos.intValue].transform.position;
        }
    }

    private void DrawTiles()
    {
        // for each tile of the generated maze
        
        for (int y = 0; y < height.intValue; y++)
        {
            for (int x = 0; x < width.intValue; x++)
            {
                // store position to access child index at end of loop
                int pos = y * width.intValue + x;

                // get position from current tile in loop
                Vector2Int gridPosition = new Vector2Int(x, y);
                
                // instantiate map tile at fitting position as child of the map canvas
                mapTile.InstantiateTileAt(gridPosition, mapParent);
                
                // add the map tile to the map parent
                shownTiles.Add(mapParent.GetChild(pos+3).gameObject);
            }
        }
    }

    private void mapActivated(InputAction.CallbackContext obj)
    {
        // open map on call if closed and close if opened
        mapParent.gameObject.SetActive(!mapParent.gameObject.activeSelf);
    }
}