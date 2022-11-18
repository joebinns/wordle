using UnityEngine;

namespace Tilemaps
{
    public class LetterTilemapHandler : TilemapHandler
    {
        protected override void ResetTile(Vector3Int position)
        {
            Tilemap.SetTile(position, null);
        }
    }
}
