using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] protected TileList_SO tileList;
        [Tooltip("Width of generated maze in number of tiles (x-axis).")]
        [SerializeField] private IntVariable width;
        [Tooltip("Height of generated maze in number of tiles (y-axis).")]
        [SerializeField] private IntVariable height;
        [SerializeField] private List<MazeModifier> mazeModifiers;

        [Header("InstantiationParent")] 
        [Tooltip("Transform parent of all generated tile sprites.")]
        [SerializeField] private Transform tileParentTransform;
        [SerializeField] private Transform interactableParentTransform;
        
        [Header("Directions")]
        [SerializeField] private Vector2IntVariable north;
        [SerializeField] private Vector2IntVariable south;
        [SerializeField] private Vector2IntVariable east;
        [SerializeField] private Vector2IntVariable west;

        [Header("Position tracking")]
        [Tooltip("Variable for spawning player and working with player position.")]
        [SerializeField] private Vector2IntVariable playerPos;
        [Tooltip("Variable for spawning hunter and working with hunter position.")]
        [SerializeField] private Vector2IntVariable hunterPos;
        [Tooltip("Variable working with hatch position.")]
        [SerializeField] private Vector2IntVariable hatchPosition;
        
        [Header("Events")]
        [Tooltip("Game Event for player spawn position.")]
        [SerializeField] private GameEvent onMazeGenerationComplete;
        
        public Transform GetInteractableInstantiationParent() => interactableParentTransform;

        private Tile[] _tiles;
        private List<Edge> _edges;

        public void Awake()
        {
            // maze Seed Generation
            int seed = randomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : setSeed;
            Random.InitState(seed);
            Debug.Log($"The used seed is: {seed.ToString()}" +
                      $"  |  Copy the seed into the setSeed field of the MazeGenerator and put the randomizeSeed boolean to false. " +
                      $"By that you get the same maze. Stop the game before though - else it wont save your changes inside the MazeGenerator!");

            //Generate the Maze
            KruskalAlgorithm();
            DrawTiles();

            List<TileBehaviour> tilesWithoutInteractable = tileList.ToList();
            Debug.Log(tilesWithoutInteractable.Count);
            SetPositions(tilesWithoutInteractable);
            Debug.Log(tilesWithoutInteractable.Count);
            foreach (var mazeModifier in mazeModifiers)
            {
                mazeModifier.AddInteractableModifier(this, tilesWithoutInteractable);
            }
            Debug.Log(tilesWithoutInteractable.Count);
            
            MazeRendererBehaviour mazeRenderer = GetComponent<MazeRendererBehaviour>();
            if (mazeRenderer != null)
            {
                mazeRenderer.InitializeMazeRenderer();
            }
            
            // initialize events
            onMazeGenerationComplete.Raise();
        }
        
        private void SetPositions(List<TileBehaviour> tilesWithoutInteractable)
        {
            //player pos
            Vector2Int newPlayerPos = new Vector2Int(Mathf.RoundToInt(Random.Range(0f, width.Get() - 1)),
                Mathf.RoundToInt(Random.Range(0f, height.Get() - 1)));
            playerPos.Set(newPlayerPos);
            tilesWithoutInteractable.Remove(tileList.GetTileAt(newPlayerPos));
            
            //hunter pos
            hunterPos.Set(playerPos.Get());
            
            //Set the Hatch Position at the opposite of the starting position.
            int hatchX = width.Get() - 1 - playerPos.Get().x; 
            int hatch = height.Get() - 1 - playerPos.Get().y;
            Vector2Int hatchPos = new Vector2Int(hatchX, hatch);
            hatchPosition.Set(hatchPos);
            tilesWithoutInteractable.Remove(tileList.GetTileAt(hatchPos));
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
                    tile.InstantiateTileAt(_tiles[gridPosition], tileParentTransform);
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