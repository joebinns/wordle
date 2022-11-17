using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapUtilities
{
    public static Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;
}
