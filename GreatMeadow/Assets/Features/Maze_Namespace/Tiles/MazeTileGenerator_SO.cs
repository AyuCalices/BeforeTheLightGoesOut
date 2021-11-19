using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class MazeTileGenerator_SO : TileGenerator_SO
    {
        [SerializeField] private SpriteRenderer tilePrefab;

        public void InstantiateTileAt(Vector2 position, Transform tileParent, Transform grassSpriteParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tileList.GetTileAt((int)position.x, (int)position.y).directions);
            
            SpriteRenderer grassSprite = Instantiate(tilePrefab, grassSpriteParent);
            grassSprite.transform.localPosition = position;
            grassSprite.sprite = tileSprite.grassSprite;
            
            GameObject tile = Instantiate(tileSprite.tile, tileParent);
            tile.transform.localPosition = position;
        }
    }
}
