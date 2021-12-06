using System.Collections.Generic;
using DataStructures.Variables;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Event_Namespace;
using Random = UnityEngine.Random;

namespace Features.Maze_Namespace
{
    public class MazeGenerator_Behaviour : MonoBehaviour
    {
        [Header("Seed")]
        [Tooltip("Chosen Seed.")]
        [SerializeField] private int setSeed = 12345;
        [Tooltip("If put to false it will use the setSeed value above. Else, it will randomly generate a seed.")]
        [SerializeField] private bool randomizeSeed;

        [Header("Appearance")] 
        [SerializeField] private MazeTileGenerator_SO tile;
        [Tooltip("Width of generated maze in number of tiles (x-axis).")]
        [SerializeField] private IntVariable width;
        [Tooltip("Height of generated maze in number of tiles (y-axis).")]
        [SerializeField] private IntVariable height;

        [Header("InstantiationParent")] 
        [Tooltip("Transform parent of all generated tile sprites.")]
        [SerializeField] private Transform tileParentTransform;
        
        [Header("Directions")]
        [SerializeField] private Vector2IntVariable north;
        [SerializeField] private Vector2IntVariable south;
        [SerializeField] private Vector2IntVariable east;
        [SerializeField] private Vector2IntVariable west;

        [Header("Tiles")]
        [Tooltip("List of tiles to be spawned.")]
        [SerializeField] private TileList_SO tiles;
        [Tooltip("Determine the number of neighbor tiles to be rendered.")]
        [SerializeField] private int renderSize = 1;
        [Tooltip("Grants access to the neighbor tiles' information.")]
        [SerializeField] private PositionController posControl;
        
        [Header("Position tracking")]
        [Tooltip("Variable for spawning player and working with player position.")]
        [SerializeField] private Vector2IntVariable playerPos;
        [Tooltip("Variable for spawning hunter and working with hunter position.")]
        [SerializeField] private Vector2IntVariable hunterPos;
        
        [Header("Events")]
        [Tooltip("Game Event for player spawn position.")]
        [SerializeField] private GameEvent onPlaceCharacter;
        [Tooltip("Game Event for hatch spawn position.")]
        [SerializeField] private GameEvent onInitializeHunter;
        [Tooltip("Game Event for hatch spawn position.")]
        [SerializeField] private GameEvent onPlaceHatch;
        [Tooltip("Game Event for hatch spawn position.")]
        [SerializeField] private GameEvent onInitializeMap;
        
        [Header("Places Interaction objects")]
        [Tooltip("Transform parent of all generated torches.")]
        [SerializeField] private Transform torchParentTransform;
        [SerializeField] private InteractableTorch torchPrefab;
        

        private Tile[] _tiles;
        private List<Edge> _edges;
        
        // list of tiles to be spawned
        private List<TileBehaviour> runtimeTiles = new List<TileBehaviour>();

        private List<TileBehaviour> oldRenderTiles = new List<TileBehaviour>();

        private bool isInitialized;
        private Vector2Int lastTilePosition;

        public void Awake()
        {
            // maze Seed Generation
            int seed = randomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : setSeed;
            Random.InitState(seed);
            Debug.Log($"The used seed is: {seed.ToString()}" +
                      $"  |  Copy the seed into the setSeed field of the MazeGenerator and put the randomizeSeed boolean to false. " +
                      $"By that you get the same maze. Stop the game before though - else it wont save your changes inside the MazeGenerator!");
            
            // randomize player starting position
            //playerPos.vec2Value = new Vector2(Mathf.Round(Random.Range(0f, width.intValue - 1)), Mathf.Round(Random.Range(0f, height.intValue - 1)));

            // generate the Maze
            KruskalAlgorithm();

            // draw the maze (tile objects)
            DrawTiles();
            
            // randomize player starting position
            playerPos.Set(new Vector2Int(Mathf.RoundToInt(Random.Range(0f, width.Get() - 1)), Mathf.RoundToInt(Random.Range(0f, height.Get() - 1))));
            hunterPos.Set(playerPos.Get());
            
            
            //TODO: Over the top: create a genertator modifier interface inside a scriptable object and implement placeCharacter, placeHatch, initializeMap into those ?
            // initialize events
            onPlaceCharacter.Raise();
            onInitializeHunter.Raise();
            onPlaceHatch.Raise();
            onInitializeMap.Raise();
            PlaceTorchesInMaze();
            
            
            
            //TODO: drop the dynamic rendering in another script
            //TODO: create some rendering methods/class for below
            // start with unrendered tiles & grass art to save performance
            OptimizeRender();
            lastTilePosition = playerPos.Get();
            
            isInitialized = true;
        }

        public void Update()
        {
            //  WIP (implement only optimizeRender when player location tile changed)
            if (!isInitialized) return;
            
            if (lastTilePosition != playerPos.Get()) 
            {
                OptimizeRender();
                lastTilePosition = playerPos.Get();
            }
        }

        #region Kruskal Algorithm
    
        /// <summary>
        /// The used maze generator is based on the Kruskal Algorithm: https://weblog.jamisbuck.org/2011/1/3/maze-generation-kruskal-s-algorithm.
        /// 
        /// The required data structure is a tree for the tiles. Additionally all edges between the tiles, including their two tiles they are neighbors to.
        /// 
        /// 1. Throw all the edges in a List
        /// 2. Step randomly over all edges and remove that edge from the List. If the edge connects two tiles from different trees, connect them. Otherwise, continue with the next edge.
        /// 3. Continue till there are no more Edges Left
        /// </summary>
        private void KruskalAlgorithm()
        {
            //Tile Initialization
            _tiles = new Tile[width.Get() * height.Get()];
            for (int i = 0; i < width.Get() * height.Get(); i++)
            {
                _tiles[i] = new Tile(i % width.Get(), i / width.Get());
            }
        
            //Edge Initialization
            _edges = new List<Edge>();
            GenerateEdgesVertical();
            GenerateEdgesHorizontal();
            
            //Some Kruskal-Magic
            int loopNum = _edges.Count;
            for (int i = 0; i < loopNum; i++)
            { 
                ConnectTile();
            }
        }

        /// <summary>
        ///      Edge
        /// _______|_______
        /// | Tile | Tile |
        /// |______|______|
        ///        |
        /// </summary>
        private void GenerateEdgesVertical()
        {
            int edgeIndex = 0;
            for (int y = 1; y <= height.Get(); y++)
            {
                for (int x = 1; x < width.Get(); x++)
                {
                    Edge edge = new Edge(2);
                    _edges.Add(edge);
                
                    edge.tiles[0] = _tiles[edgeIndex];
                    edge.tiles[1] = _tiles[edgeIndex+1];
                
                    edgeIndex++;
                }
                edgeIndex++;
            }
        }
        
        /// <summary>
        ///   ________
        ///   | Tile |
        /// __|______|__ Edge
        ///   | Tile |
        ///   |______|
        ///        
        /// </summary>
        private void GenerateEdgesHorizontal()
        {
            int edgeIndex = 0;
            for (int y = 1; y < height.Get(); y++)
            {
                for (int x = 0; x < width.Get(); x++)
                {
                    Edge edge = new Edge(2);
                    _edges.Add(edge);

                    edge.tiles[0] = _tiles[edgeIndex];
                    edge.tiles[1] = _tiles[edgeIndex + width.Get()];

                    edgeIndex++;
                }
            }
        }

        private void ConnectTile()
        {
            //Get a random edge and then remove it from the remaining edge-pool
            int randInt = Random.Range(0, _edges.Count);
            Edge randomEdge = _edges[randInt];
            _edges.RemoveAt(randInt);

            //Return if both are connected to each other
            if (Tile.GetHighestParent(randomEdge.tiles[1]) == Tile.GetHighestParent(randomEdge.tiles[0])) return;
        
            //Connect tiles to the same collection
            Tile.GetHighestParent(randomEdge.tiles[1]).parent = randomEdge.tiles[0];
            SetTileDirections(randomEdge);
        }

        /// <summary>
        /// Set the directions where the player can move to (inside the tiles from the passed edge)
        /// </summary>
        /// <param name="edge">The current selected edge by the Kruskal Algorithm</param>
        private void SetTileDirections(Edge edge)
        {
            Tile firstTile = edge.tiles[0];
            Tile secondTile = edge.tiles[1];

            int x = firstTile.position.x - secondTile.position.x;
            SetDirectionsByAxis(x, edge.tiles, east, west);
        
            int y = firstTile.position.y - secondTile.position.y;
            SetDirectionsByAxis(y, edge.tiles, north, south);
        }

        private void SetDirectionsByAxis(int axis, Tile[] tiles, Vector2IntVariable positiveDirection, Vector2IntVariable negativeDirection)
        {
            switch (axis)
            {
                case 1:
                    tiles[0].directions.Add(negativeDirection);
                    tiles[1].directions.Add(positiveDirection);
                    break;
                case -1:
                    tiles[0].directions.Add(positiveDirection);
                    tiles[1].directions.Add(negativeDirection);
                    break;
            }
        }
        #endregion
    
        /// <summary>
        /// Draw the maze based on the tile directions set inside the KruskalAlgorithm.
        /// </summary>
        private void DrawTiles()
        {
            // for all tiles to be generated according to chosen height & width
            for (int y = 0; y < height.Get(); y++)
            {
                for (int x = 0; x < width.Get(); x++)
                {
                    // create position grid with current tile position in loop
                    int gridPosition = y * width.Get() + x;
                    
                    // instantiate the current tile & art at position as children of the determined parent transforms
                    TileBehaviour newTile = tile.InstantiateTileAt(_tiles[gridPosition], tileParentTransform);
                    
                    // add generated tile to list of objects to be (de)-spawned
                    runtimeTiles.Add(newTile);
                }
            }
        }

        //TODO: out
        private void OptimizeRender()
        {
            List<TileBehaviour> newRenderTiles = new List<TileBehaviour>();

            // initialize tiles to be rendered
            List<TileBehaviour> tilesToRender = new List<TileBehaviour>();
            
            // get a list of tiles to be rendered at the current position
            tilesToRender = posControl.GetPathsByDepth(tiles.GetTileAt(playerPos.Get()), renderSize);

            // go through the list of tiles to be rendered
            foreach(TileBehaviour renderedTile in tilesToRender)
            {
                // render the tile at the given position
                if (!oldRenderTiles.Contains(renderedTile))
                {
                    renderedTile.Enable();
                }
                else
                {
                    oldRenderTiles.Remove(renderedTile);
                }

                newRenderTiles.Add(renderedTile);
            }
            
            foreach (var renderTile in oldRenderTiles)
            {
                renderTile.Disable();
            }

            oldRenderTiles = newRenderTiles;
        }

        //TODO: out
        private void PlaceTorchesInMaze()
        {
            //calculate percentage of torch placement
            int amountOfTorches = Mathf.RoundToInt((height.Get() * width.Get()) * .10f);
            Vector2[] torchesToPlace = new Vector2[amountOfTorches];

            for (int torchNr = 0; torchNr < torchesToPlace.Length; torchNr++)
            {
                Vector2Int torchPosition = new Vector2Int(Mathf.RoundToInt(Random.Range(0f, width.Get() - 1)), Mathf.RoundToInt(Random.Range(0f, height.Get() - 1)));
                
                if (!runtimeTiles[torchPosition.y * width.Get() + torchPosition.x].ContainsInteractable() && torchPosition != playerPos.Get())
                {
                    torchesToPlace[torchNr] = torchPosition;
                    
                    InteractableTorch torch = Instantiate(torchPrefab, torchParentTransform);
                    torch.transform.position = (Vector2)torchPosition;
                    
                    runtimeTiles[torchPosition.y * width.Get() + torchPosition.x].RegisterInteractable(torch);
                }
            }
        }

        [System.Serializable]
        public class Edge
        {
            public Tile[] tiles;

            public Edge(int tileSize)
            {
                tiles = new Tile[tileSize];
            }
        }
    }

    [System.Serializable]
    public class Tile
    {
        public Tile parent;
        public Vector2Int position;
        public List<Vector2IntVariable> directions = new List<Vector2IntVariable>();

        public Tile(int x, int y)
        {
            position = new Vector2Int(x, y);
        }
    
        public static Tile GetHighestParent(Tile tile)
        {
            while (true)
            {
                if (tile.parent == null) return tile;
                tile = tile.parent;
            }
        }
    }
}