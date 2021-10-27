using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class TileSprite_SO : ScriptableObject
    {
        public Sprite sprite;
        public GameObject shadowCaster;
        public List<Vector2Variable> directions;
    }
}
