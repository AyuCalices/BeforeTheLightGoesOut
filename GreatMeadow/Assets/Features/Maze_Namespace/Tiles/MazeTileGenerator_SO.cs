using UnityEngine;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class MazeTileGenerator_SO : TileGenerator_SO
    {
        public void InstantiateTileAt(Tile tile, Transform tileParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tile.directions);

            TileBehaviour runtimeTile = Instantiate(tileSprite.tile, tileParent);
            runtimeTile.Initialize(tile);
            runtimeTile.transform.localPosition = (Vector2)tile.position;
        }
    }
}
