using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class LetterTilemapTracker : MonoBehaviour
    {
        // Name --> Position

        private Tilemap _tilemap;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        private Dictionary<string, Vector3Int> _tileNameToPosition = new Dictionary<string, Vector3Int>();
        public Vector3Int TileNameToPosition(string name) => _tileNameToPosition[name];
        public string PositionToTileName(Vector3Int position) => _tileNameToPosition.FirstOrDefault(x => x.Value == position).Key;

        private Dictionary<string, Vector3Int> MapTileNamesToPositions()
        {
            var tileNameToPosition = new Dictionary<string, Vector3Int>();
            for (int x = 0; x < _tilemap.size.x; x++)
            {
                for (int y = 0; y < _tilemap.size.y; y++)
                {
                    for (int z = 0; z < _tilemap.size.z; z++)
                    {
                        var position = new Vector3Int(x, -y, z);
                        var tile = _tilemap.GetTile(position);
                        if (tile == null) { continue; }
                        var name = tile.name;
                        tileNameToPosition[name] = position;
                    }
                }
            }
            return tileNameToPosition;
        }
    }
}
