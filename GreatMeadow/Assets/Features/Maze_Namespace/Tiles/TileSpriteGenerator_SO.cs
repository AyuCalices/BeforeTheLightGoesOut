using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class TileSpriteGenerator_SO : ScriptableObject
    {
        [SerializeField] private SpriteRenderer tilePrefab;
        [SerializeField] private TileSprite_SO[] tileSprites;

        private TileSprite_SO GetTileSpriteByDirections(List<Vector2Variable> directions)
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

        public void InstantiateTileAt(Vector2 position, Transform spriteParent, List<Vector2Variable> directions, Transform shadowCastParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(directions);
            
            SpriteRenderer instantiatedTile = Instantiate(tilePrefab, spriteParent);
            instantiatedTile.transform.localPosition = position;
            instantiatedTile.sprite = tileSprite.sprite;
            
            GameObject shadowCaster = Instantiate(tileSprite.shadowCaster, shadowCastParent);
            shadowCaster.transform.localPosition = position;
        }
public void InstantiateMapTileAt(Vector2 position, List<Vector2Variable> directions, Transform tileParent)
        {
            TileSprite_SO tileSprite = GetTileSpriteByDirections(directions);  
            GameObject tile = Instantiate(tileSprite.tile, tileParent);
            tile.transform.localPosition = position;
        }

    }

}