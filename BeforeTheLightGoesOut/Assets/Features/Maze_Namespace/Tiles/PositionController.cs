using System.Collections.Generic;
using UnityEngine;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class PositionController : ScriptableObject
    {
        public TileList_SO tiles;

        /// <summary>
        /// </summary>
        /// <param name="startTile">The position the search will start from.</param>
        /// <returns>Returns all tiles around a passed tile. It will not contain the tile passed by the parameter</returns>
        public List<TileBehaviour> GetConnectedTiles(TileBehaviour startTile)
        {
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>();
            
            for (int n = 0; n < startTile.directions.Count; n++)
            {
                Vector2Int neighborTile = startTile.position;
                neighborTile += startTile.directions[n].Get();
                connectedTiles.Add(tiles.GetTileAt(neighborTile.x, neighborTile.y));
            }
            
            return connectedTiles;
        }

        /// <summary>
        /// Returns all positions around one tile by a certain depth.
        /// </summary>
        /// <param name="startTile">The position the search will start from.</param>
        /// <param name="depth">The amount of tiles away from the startPosition</param>
        /// <returns>Returns all tiles which tile distance is <= depth</returns>
        public List<TileBehaviour> GetPathsByDepth(TileBehaviour startTile, int depth)
        {
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>{startTile};
            
            List<TileBehaviour> currentDepthTiles = new List<TileBehaviour>{startTile};
            
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

        /// <summary>
        /// </summary>
        /// <param name="startTile">The position the search will start from</param>
        /// <param name="depth">The amount of tiles away from the startPosition</param>
        /// <param name="tileToExclude">The direction parent tile of startTile</param>
        /// <returns>Returns a list of tiles based of a startingTilePosition and excludes the previous tile.</returns>
        public List<TileBehaviour> GetPathsByDepth_ExcludeParentPositions(TileBehaviour startTile, int depth, TileBehaviour tileToExclude)
        {
            if (!GetConnectedTiles(tileToExclude).Contains(startTile))
            {
                Debug.LogError($"The {tileToExclude} is not a direction parent of {startTile}. Please pass a valid tile to exclude!");
            }
            
            List<TileBehaviour> connectedTiles = new List<TileBehaviour>{startTile};
            
            Dictionary<TileBehaviour, List<TileBehaviour>> currentDepthTileGroups = new Dictionary<TileBehaviour, List<TileBehaviour>>();
            currentDepthTileGroups.Add(tileToExclude, new List<TileBehaviour>{startTile});
            
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
