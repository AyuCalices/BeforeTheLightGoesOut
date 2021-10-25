using System.Collections.Generic;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace
{
    public class MazeGenerator_Behaviour : MonoBehaviour
    {
        [Header("Seed")]
        [SerializeField] private int setSeed = 12345;
        [Tooltip("If put to false it will use the setSeed value above. Else it will randomly generate a seed.")]
        [SerializeField] private bool randomizeSeed;

        [Header("Appearance")] 
        [SerializeField] private TileSpriteGenerator_SO tileSprites;
        [SerializeField] private IntVariable width;
        [SerializeField] private IntVariable height;

        [Header("Directions")]
        [SerializeField] private Vector2Variable north;
        [SerializeField] private Vector2Variable south;
        [SerializeField] private Vector2Variable east;
        [SerializeField] private Vector2Variable west;

        [Header("Tiles")]
        [SerializeField] private TileList_SO tiles;

        private Tile[] _tiles;
        private List<Edge> _edges;

        public void Start()
        {
            //Maze Seed Generation
            int seed = randomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : setSeed;
            Random.InitState(seed);
            Debug.Log($"The used seed is: {seed.ToString()}" +
                      $"  |  Copy the seed into the setSeed field of the MazeGenerator and put the randomizeSeed boolean to false. " +
                      $"By that you get the same maze. Stop the game before though - else it wont save your changes inside the MazeGenerator!");

            //Generate the Maze
            KruskalAlgorithm();
            DrawTiles();
            
            //Set tiles for later use
            tiles.SetTiles(_tiles);
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
                    int tilePos = y * width.intValue + x;
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    tileSprites.InstantiateTileAt(gridPosition, transform, _tiles[tilePos].directions);
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