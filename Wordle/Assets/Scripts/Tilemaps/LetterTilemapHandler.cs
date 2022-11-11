using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class LetterTilemapHandler : TilemapHandler
    {

        /*
        public bool IsWithinTilemap(Vector3Int position)
        {
            var isWithinTilemap = true;
            var i = 0;
            while (isWithinTilemap & i < 3)
            {
                isWithinTilemap = position[i] >= _tilemap.origin[i] & position[i] < _tilemap.size[i]; // TODO: Check that _tilemap.origin is as expected (i.e. (0, 0, 0)).
                i += 1;
            }
            return isWithinTilemap;
        }
        */
        
        protected override void ResetTile(Vector3Int position)
        {
            Tilemap.SetTile(position, null);
        }
    }
}
