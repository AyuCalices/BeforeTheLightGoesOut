using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    public abstract class TileGenerator_SO : ScriptableObject
    {
        [SerializeField] protected TileSprite_SO[] tileSprites;
        [SerializeField] protected TileList_SO tileList;

        protected TileSprite_SO GetTileSpriteByDirections(List<Vector2Variable> directions)
        {
            foreach (TileSprite_SO tileSprite in tileSprites)
            {
                if (tileSprite.directions.Count != directions.Count) continue;

                bool containsSameElements = directions.All(direction => tileSprite.directions.Contains(direction));
                if (containsSameElements)
                {
                    return tileSprite;
                }
            }

            Debug.LogError("There doesn't exists a tile with the injected direction List. Maybe you have removed a tile by accident?");
            return null;
        }
    }
}
