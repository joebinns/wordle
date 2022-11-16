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
            _characterToPosition = MapTileNamesToPositions();
        }

        private Dictionary<char, Vector3Int> _characterToPosition = new Dictionary<char, Vector3Int>();
        public Vector3Int CharacterToPosition(char character) => _characterToPosition[character];
        public char PositionToCharacter(Vector3Int position) => _characterToPosition.FirstOrDefault(x => x.Value == position).Key;
        public bool Contains(char character) => _characterToPosition.ContainsKey(character);
        
        public char TileNameToCharacter(string name)
        {
            char character = '\0';
            if (name != null)
            {
                character = name[0];
                if (name == "enter")
                {
                    character = '\r'; // WARNING: This will miss '\n'
                }
                else if (name == "backspace")
                {
                    character = '\b';
                }
            }
            return character;
        }

        private Dictionary<char, Vector3Int> MapTileNamesToPositions()
        {
            var tileNameToPosition = new Dictionary<char, Vector3Int>();
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
                        var character = TileNameToCharacter(name);
                        tileNameToPosition[character] = position;
                    }
                }
            }
            return tileNameToPosition;
        }
    }
}
