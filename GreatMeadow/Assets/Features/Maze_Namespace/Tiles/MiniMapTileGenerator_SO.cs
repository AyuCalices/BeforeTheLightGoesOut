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
        public GameObject InstantiateTileAt(Vector2Int position, Transform tileParent, float spriteSize, int mazeWidth, int mazeHeight)
        {
            // get tile positions and directions
            TileSprite_SO tileSprite = GetTileSpriteByDirections(tileList.GetTileAt(position).directions);
            
            // instantiate sprite under parent
            Image grassSprite = Instantiate(imagePrefab, tileParent);
            
            // get canvas positions
            RectTransform imagePrefabRect = grassSprite.GetComponent<RectTransform>();
            
            // place sprite at correct position
            imagePrefabRect.sizeDelta = new Vector2(spriteSize, spriteSize);
            var rect = imagePrefabRect.rect;
            float posX = (position.x - (mazeWidth / 2f)) * rect.width + rect.width / 2f;
            float posY = (position.y - (mazeHeight / 2f)) * rect.height + rect.height / 2f;
            grassSprite.transform.localPosition = new Vector3(posX, posY); 
           
            // load fitting minimap sprite
            grassSprite.sprite = tileSprite.miniMapSprite;

            return grassSprite.gameObject;
        }
    }
}
