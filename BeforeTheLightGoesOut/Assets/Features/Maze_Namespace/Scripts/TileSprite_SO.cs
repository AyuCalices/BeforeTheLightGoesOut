using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;
using Utils.Variables;

namespace Features.Maze_Namespace.Scripts
{
    [CreateAssetMenu]
    public class TileSprite_SO : ScriptableObject
    {
        public Sprite miniMapSprite;
        public TileBehaviour tile;
        public List<Vector2IntVariable> directions;
    }
}
