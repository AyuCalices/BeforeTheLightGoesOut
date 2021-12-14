using DataStructures.Variables;
using Features.Maze_Namespace.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Variables;

namespace Features.Map_Namespace.Scripts
{
    public class MapBehaviour : MonoBehaviour
    {
        [Tooltip("Width of generated maze in number of tiles (x-axis).")]
        [SerializeField] private IntVariable width;
        [Tooltip("Height of generated maze in number of tiles (y-axis).")]
        [SerializeField] private IntVariable height;

        [Tooltip("Parent transform of all map tiles children.")]
        [SerializeField] private GameObject map;
        [SerializeField] private RectTransform mapInstantiationParent;
        [SerializeField] private Transform playerPositionPoint;
    
        [Tooltip("Grants access to the generation of the map.")]
        [SerializeField] private MiniMapTileGenerator_SO mapTile;

        // grants access to player input
        private PlayerInputActions playerInputActions;
    
        // handles input actions on the map scene
        private InputAction mapHandling;
    
        // list to access game object (de-)activation as part of map progression
        private GameObject[][] mapTiles;
    
        // access to the player positions' tile number
        [SerializeField] private Vector2IntVariable playerPosition;

        private bool isInitialized;
    
        public void InitializeMap()
        {
            // draw map tiles from the same maze seed
            DrawTiles();
        
            for (int y = 0; y < height.Get(); y++)
            {
                for (int x = 0; x < width.Get(); x++)
                {
                    mapTiles[y][x].SetActive(false);
                }
            }

            // depict player position marker above all tiles
            playerPositionPoint.SetAsLastSibling();

            // only start update, when initialization is completed
            isInitialized = true;
        }
    
        public void ToggleMap()
        {
            // open map on call if closed and close if opened
            map.SetActive(!map.activeSelf);
        }
    
        private void Awake()
        {
            // initialize input actions for map scene
            playerInputActions = new PlayerInputActions();
        
            // enable player input in map scene
            playerInputActions.Enable();
        
            map.SetActive(false);
        }
    
        private void OnEnable()
        {
            // allow opening of the map
            playerInputActions.Player.OpenMap.performed += MapActivated;
            playerInputActions.Player.OpenMap.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Player.OpenMap.performed -= MapActivated;
            playerInputActions.Player.OpenMap.Disable();
        }

        private void Update()
        {
            if (isInitialized) 
            {
                // reveal map tile at player position
                Vector2Int pos = playerPosition.Get();
            
                mapTiles[pos.y][pos.x].SetActive(true);
            
                // depict player position on current map tile
                playerPositionPoint.position = mapTiles[pos.y][pos.x].transform.position;
            }
        }

        private void DrawTiles()
        {
            float size = Mathf.Min(mapInstantiationParent.rect.width / width.Get(),
                mapInstantiationParent.rect.height / height.Get());
        
            // for each tile of the generated maze
            mapTiles = new GameObject[height.Get()][];
            for (int y = 0; y < height.Get(); y++)
            {
                mapTiles[y] = new GameObject[width.Get()];
                for (int x = 0; x < width.Get(); x++)
                {
                    // get position from current tile in loop
                    Vector2Int gridPosition = new Vector2Int(x, y);
                
                    // add the map tile to the map parent
                    mapTiles[y][x] = mapTile.InstantiateTileAt(gridPosition, mapInstantiationParent, size, width.Get(), height.Get());
                }
            }
        }

        private void MapActivated(InputAction.CallbackContext obj)
        {
            // open map on call if closed and close if opened
            map.SetActive(!map.activeSelf);
        }
    }
}