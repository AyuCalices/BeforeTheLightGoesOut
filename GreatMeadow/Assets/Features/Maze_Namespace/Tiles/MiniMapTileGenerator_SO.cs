using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class MiniMapTileGenerator_SO : TileGenerator_SO
    {
        [SerializeField] private Image imagePrefab;

        public void InstantiateTileAt(Vector2 position, Transform tileParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tileList.GetTileAt((int)position.x, (int)position.y).directions);
            
            Image grassSprite = Instantiate(imagePrefab, tileParent);
            grassSprite.transform.localPosition = position;
            grassSprite.sprite = tileSprite.miniMapSprite;
        }
    }
}
