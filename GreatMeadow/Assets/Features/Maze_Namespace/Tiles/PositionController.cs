using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class PositionController : ScriptableObject
    {
        public TileList_SO tiles;

        public List<TileBehaviour> GetConnectedTiles(TileBehaviour tile)
        {
            // create list for saving all connected floors to the given length
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>();
            
            // go through the list of vec2s (connected floors) of the given tile
            for (int n = 0; n < tile.directions.Count; n++)
            {
                // get neighboring tile position from given direction
                Vector2Int neighborTile = tile.position;
                neighborTile += new Vector2Int((int)tile.directions[n].vec2Value.y, (int)tile.directions[n].vec2Value.x);

                // add connected tile to list of connections
                connectedTiles.Add(tiles.GetTileAt((int) neighborTile.x, (int) neighborTile.y));
            }
            
            return connectedTiles;
        }

        public List<TileBehaviour> GetPathsByDepth(TileBehaviour tile, int depth)
        {
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>{tile};
            
            List<TileBehaviour> currentDepthTiles = new List<TileBehaviour>{tile};
            
            for (int i = 0; i < depth; i++)
            {
                List<TileBehaviour> newDepthTiles = new List<TileBehaviour>();
                foreach (var tileByDepth in currentDepthTiles)
                {
                    newDepthTiles.AddRange(GetConnectedTiles(tileByDepth));
                }

                currentDepthTiles = newDepthTiles;

                //GetConnectedTiles() also returns tiles that already have been added in the previous depth iteration.
                //That's why we have to check if it exists in the main List already.
                foreach (var newTile in newDepthTiles)
                {
                    if (!connectedTiles.Contains(newTile))
                    {
                        connectedTiles.Add(newTile);
                    }
                }
            }
            
            return connectedTiles;
        }

        public List<TileBehaviour> GetPathsByDepth_ExcludeParentPositions(TileBehaviour tile, int depth, TileBehaviour tileToExclude)
        {
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>{tile};
            
            Dictionary<TileBehaviour, List<TileBehaviour>> currentDepthTileGroups = new Dictionary<TileBehaviour, List<TileBehaviour>>();
            currentDepthTileGroups.Add(tileToExclude, new List<TileBehaviour>{tile});
            
            for (int i = 0; i < depth; i++)
            {
                Dictionary<TileBehaviour, List<TileBehaviour>> newDepthTileGroups = new Dictionary<TileBehaviour, List<TileBehaviour>>();
                foreach (var tileGroupByDepth in currentDepthTileGroups)
                {
                    
                    List<TileBehaviour> newTileDirections = new List<TileBehaviour>();
                    foreach (var newtile in tileGroupByDepth.Value)
                    {
                        List<TileBehaviour> tilesToBeAdded = GetConnectedTiles(newtile);
                        tilesToBeAdded.Remove(tileGroupByDepth.Key);
                        
                        newDepthTileGroups.Add(newtile, tilesToBeAdded);
                        connectedTiles.AddRange(tilesToBeAdded);
                    }
                }
                currentDepthTileGroups = newDepthTileGroups;
            }
            
            return connectedTiles;
        }
    }
}
