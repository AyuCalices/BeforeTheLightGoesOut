using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class TileSprite_SO : ScriptableObject
    {
        public Sprite miniMapSprite;
        public TileBehaviour tile;
        public List<Vector2IntVariable> directions;
    }
}
