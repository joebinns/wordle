using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardTilemap : MonoBehaviour
{
    public Tilemap Tilemap;
    public Dictionary<string, Vector3Int> TileNameToPosition = new Dictionary<string, Vector3Int>();

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        TileNameToPosition = MapTileNamesToPositions();
    }

    public string PositionToTileName(Vector3Int position)
    {
        var name = TileNameToPosition.FirstOrDefault(x => x.Value == position).Key;
        return name;
    }
    
    private Dictionary<string, Vector3Int> MapTileNamesToPositions()
    {
        var tileNameToPosition = new Dictionary<string, Vector3Int>();
        for (int x = 0; x < Tilemap.size.x; x++)
        {
            for (int y = 0; y < Tilemap.size.y; y++)
            {
                for (int z = 0; z < Tilemap.size.z; z++)
                {
                    var position = new Vector3Int(x, -y, z);
                    var tile = Tilemap.GetTile(position);
                    if (tile == null) { continue; }
                    var name = tile.name;
                    tileNameToPosition[name] = position;
                }
            }
        }
        return tileNameToPosition;
    }
}
