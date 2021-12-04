using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class MazeTileGenerator_SO : TileGenerator_SO
    {
        public void InstantiateTileAt(Vector2 position, Transform tileParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tileList.GetTileAt((int)position.x, (int)position.y).directions);

            GameObject tile = Instantiate(tileSprite.tile, tileParent);
            tile.transform.localPosition = position;
        }
    }
}
