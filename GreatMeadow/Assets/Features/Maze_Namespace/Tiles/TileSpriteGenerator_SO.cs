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
        [SerializeField] private Image tilePrefab;
        [SerializeField] private TileSprite_SO[] tileSprites;

        private float _tileSize;

        private void OnEnable()
        {
            _tileSize = tilePrefab.GetComponent<RectTransform>().rect.width;
        }

        private Sprite GetTileSpriteByDirections(List<Vector2Variable> directions)
        {
            foreach (TileSprite_SO tileSprite in tileSprites)
            {
                if (tileSprite.directions.Count != directions.Count) continue;

                bool containsSameElements = directions.All(direction => tileSprite.directions.Contains(direction));
                if (containsSameElements)
                {
                    return tileSprite.sprite;
                }
            }

            Debug.LogError("There doesn't exists a tile with the injected direction List. Maybe you have removed a tile by accident?");
            return null;
        }

        public void InstantiateTileAt(Vector2 position, Transform parent, List<Vector2Variable> directions)
        {
            Image instantiatedTile = Instantiate(tilePrefab, parent);
            instantiatedTile.transform.localPosition = (position * _tileSize);
            instantiatedTile.sprite = GetTileSpriteByDirections(directions);
        }
    }
}
