using System.Collections.Generic;
using UnityEngine;
using Utils.Variables_Namespace;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class TileList_SO : ScriptableObject
    {
        [SerializeField] private IntVariable MazeWidth;
        
        private Tile[] _tiles;

        public void SetTiles(Tile[] tiles)
        {
            _tiles = tiles;
        }

        public Tile GetPosition(int position)
        {
            return _tiles[position];
        }

        public Tile GetTileAt(int x, int y)
        {
            return _tiles[y * MazeWidth.intValue + x];
        }
    }
}
