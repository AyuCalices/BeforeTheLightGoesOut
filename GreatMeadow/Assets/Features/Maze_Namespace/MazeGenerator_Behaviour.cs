using System;
using System.Collections.Generic;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Variables_Namespace;
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
        [Tooltip("Transform parent of all generated grass sprites.")]
        [SerializeField] private Transform grassSpriteParentTransform;
        [Tooltip("Transform parent of all generated tile sprites.")]
        [SerializeField] private Transform tileParentTransform;
        
        [Header("Directions")]
        [SerializeField] private Vector2Variable north;
        [SerializeField] private Vector2Variable south;
        [SerializeField] private Vector2Variable east;
        [SerializeField] private Vector2Variable west;

        [Header("Tiles")]
        [Tooltip("List of tiles to be spawned.")]
        [SerializeField] private TileList_SO tiles;
        
        [Header("Positioning")]
        [Tooltip("Variable for spawning player and working with player position.")]
        [SerializeField] private Vector2Variable playerSpawnPos;
        [Tooltip("Variable for player position in tile.")]
        [SerializeField] private IntVariable tilePos;


        private Tile[] _tiles;
        private List<Edge> _edges;
        public List<GameObject> spawnTiles;
        public List<GameObject> grassTiles;
        [SerializeField] private int renderSize = 2;

        public void Awake()
        {
            // maze Seed Generation
            int seed = randomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : setSeed;
            Random.InitState(seed);
            Debug.Log($"The used seed is: {seed.ToString()}" +
                      $"  |  Copy the seed into the setSeed field of the MazeGenerator and put the randomizeSeed boolean to false. " +
                      $"By that you get the same maze. Stop the game before though - else it wont save your changes inside the MazeGenerator!");
            
            // randomize player starting position
            playerSpawnPos.vec2Value = new Vector2(Mathf.Round(Random.Range(0f, width.intValue - 1)), Mathf.Round(Random.Range(0f, height.intValue - 1)));

            tilePos.intValue = (int) (playerSpawnPos.vec2Value.y + 0.5) * width.intValue + (int) (playerSpawnPos.vec2Value.x + 0.5);
            
            // generate the Maze
            KruskalAlgorithm();
            
            // Set tiles for global use
            tiles.SetTiles(_tiles);
            
            spawnTiles = new List<GameObject>();
            
            // draw the maze (tile objects)
            DrawTiles();
            
            for (int n = 0; n < width.intValue*height.intValue; n++)
            {
                spawnTiles[n].SetActive(false);
                grassTiles[n].SetActive(false);
            }
            
        }

        public void Update()
        {
            tilePos.intValue = (int) (playerSpawnPos.vec2Value.y + 0.5) * width.intValue + (int) (playerSpawnPos.vec2Value.x + 0.5);
            OptimizeRender();
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
            _tiles = new Tile[width.intValue * height.intValue];
            for (int i = 0; i < width.intValue * height.intValue; i++)
            {
                _tiles[i] = new Tile(i % width.intValue, i / width.intValue);
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
            for (int y = 1; y <= height.intValue; y++)
            {
                for (int x = 1; x < width.intValue; x++)
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
            for (int y = 1; y < height.intValue; y++)
            {
                for (int x = 0; x < width.intValue; x++)
                {
                    Edge edge = new Edge(2);
                    _edges.Add(edge);

                    edge.tiles[0] = _tiles[edgeIndex];
                    edge.tiles[1] = _tiles[edgeIndex + width.intValue];

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

        private void SetDirectionsByAxis(int axis, Tile[] tiles, Vector2Variable positiveDirection, Vector2Variable negativeDirection)
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
            for (int y = 0; y < height.intValue; y++)
            {
                for (int x = 0; x < width.intValue; x++)
                {
                    int pos = y * width.intValue + x;
                    
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    tile.InstantiateTileAt(gridPosition, tileParentTransform, grassSpriteParentTransform);
                    spawnTiles.Add(tileParentTransform.GetChild(pos).gameObject);
                    grassTiles.Add(grassSpriteParentTransform.GetChild(pos).gameObject);
                }
            }
        }

        private void OptimizeRender()
        {
            //Debug.Log((int) tilePos.vec2Value.x + "  /" + (int) tilePos.vec2Value.y);
            
            for (int x = -renderSize; x <= renderSize; x++)
            {
                for (int y = -renderSize; y <= renderSize; y++)
                {
                    int renderPos = tilePos.intValue + x + y * width.intValue;
                    
                    if (renderPos >= 0 && renderPos <= width.intValue*height.intValue)
                    {
                        spawnTiles[renderPos].gameObject.SetActive(true);
                        grassTiles[renderPos].gameObject.SetActive(true);
                        
                        /*
                        if (renderPos >= 1 && renderPos <= width.intValue * height.intValue-width.intValue)
                        {
                            if (x == -renderSize) spawnTiles[renderPos - 1].gameObject.SetActive(false);
                            if (x == renderSize) spawnTiles[renderPos + 1].gameObject.SetActive(false);
                            if (y == -renderSize)
                                spawnTiles[renderPos - (1 * width.intValue)].gameObject.SetActive(false);
                            if (y == renderSize) spawnTiles[renderPos + (1 * width.intValue)].gameObject.SetActive(false);
                        }
                        */
                    }
                    
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
        public List<Vector2Variable> directions = new List<Vector2Variable>();

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