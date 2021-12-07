using UnityEngine;
using UnityEngine.UI;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class MiniMapTileGenerator_SO : TileGenerator_SO
    {
        // represents sprite graphic?
        [SerializeField] private Image imagePrefab;
        
        // instantiate tile at given position under given parent transform
        public GameObject InstantiateTileAt(Vector2Int position, Transform tileParent)
        {
            // get tile positions and directions
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tileList.GetTileAt(position).directions);
            
            // instantiate sprite under parent
            Image grassSprite = Instantiate(imagePrefab, tileParent);
            
            // get canvas positions
            Rect imagePrefabRect = imagePrefab.GetComponent<RectTransform>().rect;

            // place sprite at correct position
            grassSprite.transform.localPosition = new Vector3(position.x * imagePrefabRect.width, position.y * imagePrefabRect.height); 
           
            // load fitting minimap sprite
            grassSprite.sprite = tileSprite.miniMapSprite;

            return grassSprite.gameObject;
        }
    }
}
