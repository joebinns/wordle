using UnityEngine;
using UnityEngine.Tilemaps;

public class TextManager : MonoBehaviour
{
    private Vector3Int _gridIndex = Vector3Int.zero;
    [SerializeField] private Tile _tile;

    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetCharacterAtCaret(char character)
    {
        Debug.Log(_gridIndex);
        var tile = FindTileByCharacter(character);
        if (tile != null)
        {
            if (_gridIndex.x >= 0 & _gridIndex.x < FindObjectOfType<Grid>().Dimensions.x)
            {
                // Set tile current
                _tilemap.SetTile(_gridIndex, tile);
                _gridIndex.x++;
            }
        }
        else
        {
            if (_gridIndex.x > 0 & _gridIndex.x <= FindObjectOfType<Grid>().Dimensions.x)
            {
                // Delete previous tile
                _gridIndex.x--;
                _tilemap.SetTile(_gridIndex, null);
            }
        }
    }

    private void CheckIfTextIsValid()
    {
        // If all 5 
    }

    private Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;
}
