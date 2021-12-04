using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class PositionController : ScriptableObject
    {
        public TileList_SO tiles;

        public List<Tile> GetConnectedTiles(Tile tile)
        {
            // create list for saving all connected floors to the given length
            List<Tile> connectedTiles = new List<Tile>();
            
            // go through the list of vec2s (connected floors) of the given tile
            for (int n = 0; n < tile.directions.Count; n++)
            {
                // get neighboring tile position from given direction
                Vector2 neighborTile = tile.position;
                neighborTile += new Vector2(tile.directions[n].vec2Value.y, tile.directions[n].vec2Value.x);

                // add connected tile to list of connections
                connectedTiles.Add(tiles.GetTileAt((int) neighborTile.x, (int) neighborTile.y));
            }
            
            return connectedTiles;
        }

        public List<Tile> GetPathsByDepth(Tile tile, int depth)
        {
            List<Tile> connectedTiles = new List<Tile>{tile};
            
            List<Tile> currentDepthTiles = new List<Tile>{tile};
            
            for (int i = 0; i < depth; i++)
            {
                List<Tile> newDepthTiles = new List<Tile>();
                foreach (var tileByDepth in currentDepthTiles)
                {
                    newDepthTiles.AddRange(GetConnectedTiles(tileByDepth));
                }

                currentDepthTiles = newDepthTiles;
                connectedTiles.AddRange(newDepthTiles);
            }

            return connectedTiles;
        }
    }
}
