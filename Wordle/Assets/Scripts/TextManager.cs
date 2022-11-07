using UnityEngine;
using UnityEngine.Tilemaps;

public class TextManager : MonoBehaviour
{
    private int[] _gridIndex = { 0, 0 };
    [SerializeField] private Tile _tile;

    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetText(Vector3Int position, char character)
    {
        var tile = FindTileByCharacter(character);
        _tilemap.SetTile(position, tile);
    }

    private Tile FindTileByCharacter(char character)
    {
        var tile = Resources.Load(character.ToString()) as Tile;
        return tile;
    }
}
