using UnityEngine;
using Utils.Variables_Namespace;

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
            _tiles = new TileBehaviour[height.intValue][];
            for (int y = 0; y < _tiles.Length; y++)
            {
                _tiles[y] = new TileBehaviour[width.intValue];
            }
        }

        public void RegisterTile(TileBehaviour tileBehaviour)
        {
            _tiles[tileBehaviour.position.y][tileBehaviour.position.x] = tileBehaviour;
        }

        public TileBehaviour GetTileAt(int x, int y)
        {
            return _tiles[y][x];
        }
    }
}
