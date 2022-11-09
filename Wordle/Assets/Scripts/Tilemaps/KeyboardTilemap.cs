using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardTilemap : MonoBehaviour
{
    private Tilemap _tilemap;
    public Dictionary<string, Vector3Int> TileNameToPosition = new Dictionary<string, Vector3Int>();

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        TileNameToPosition = MapTileNamesToPositions();
    }
    
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
