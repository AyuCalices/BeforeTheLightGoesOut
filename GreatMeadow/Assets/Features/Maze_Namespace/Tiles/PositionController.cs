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
            
            // create list of all directions of current tile
            List<Vector2Variable> tileDirections = tile.directions;
            
            // add current tile to list
            connectedTiles.Add(tiles.GetTileAt(tile.position.x, tile.position.y));
            
            // go through the list of vec2s (connected floors) of the given tile
            for (int n = 0; n < tileDirections.Count; n++)
            {
                // memorize neighboring tile from given direction
                Vector2Int neighborTile = new Vector2Int((int) (tile.position.x + tile.directions[n].vec2Value.x),
                    (int) (tile.position.y + tile.directions[n].vec2Value.y));
                
                // add connected tile to list of connections
                connectedTiles.Add(tiles.GetTileAt(neighborTile.x, neighborTile.y));
            }
            
            return connectedTiles;

        }

        public void GetNeighbors()
        {
            
        }
        
    }
}
