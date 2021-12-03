using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class PositionController : ScriptableObject
    {
        public TileList_SO tiles;
        
        
        public PositionController() 
        {
            
        }

        public List<Tile> GetPaths(Tile tile, int length)
        {
            // create list for saving all connected floors to the given length
            List<Tile> connectedTiles = new List<Tile>();

            // add current tile to list
            connectedTiles.Add(tile);
            
            // go through the list of vec2s (connected floors) of the given tile
            for (int n = 0; n < tile.directions.Count; n++)
            {
                // get neighboring tile position from given direction
                Vector2 neighborTile = tile.position;
                neighborTile += new Vector2(tile.directions[n].vec2Value.y, tile.directions[n].vec2Value.x);

                // add connected tile to list of connections
                connectedTiles.Add(tiles.GetTileAt((int) neighborTile.x, (int) neighborTile.y));
            }
            
            // VARIABLE RENDER SIZE BY RECURSION WIP
            
            length = -1;
            
            if (length >= 0) {
                foreach(Tile extraTile in connectedTiles)
                {
                    connectedTiles.AddRange(GetPaths(tile, length));
                }
            }
            
            return connectedTiles;
        }

        public void GetNeighbors()
        {
            
        }
        
    }
}
