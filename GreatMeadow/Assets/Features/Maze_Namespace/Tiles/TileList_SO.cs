using System.Collections.Generic;
using System.Linq;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Maze_Namespace.Tiles
{
    [CreateAssetMenu]
    public class TileList_SO : ScriptableObject
    {
        [SerializeField] private IntVariable width;
        [SerializeField] private IntVariable height;
        
        private TileBehaviour[][] _tiles;

        private void OnEnable()
        {
            _tiles = new TileBehaviour[height.Get()][];
            for (int y = 0; y < _tiles.Length; y++)
            {
                _tiles[y] = new TileBehaviour[width.Get()];
            }
        }

        public void RegisterTile(TileBehaviour tileBehaviour)
        {
            _tiles[tileBehaviour.position.y][tileBehaviour.position.x] = tileBehaviour;
        }

        public TileBehaviour GetTileAt(int x, int y) => _tiles[y][x];

        public TileBehaviour GetTileAt(Vector2Int position) => _tiles[position.y][position.x];

        public TileBehaviour[][] GetAllTiles() => _tiles;

        public List<TileBehaviour> ToList()
        {
            List<TileBehaviour> tiles = new List<TileBehaviour>();
            
            foreach (var tilesY in _tiles)
            {
                tiles.AddRange(tilesY.ToList());
            }

            return tiles;
        }
    }
}
