using UnityEngine;
using UnityEngine.Tilemaps;

public class TextManager : MonoBehaviour
{
    private Vector3Int _gridIndex = Vector3Int.zero;
    [SerializeField] private Tile _tile;
    private bool _isEnabled = true;

    private Tilemap _tilemap;
    private Grid _grid;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _grid = FindObjectOfType<Grid>();
    }

    public void SetCharacterAtCaret(char character)
    {
        if (!_isEnabled) { return; }
        var tile = FindTileByCharacter(character);
        if (tile != null)
        {
            if (_gridIndex.x >= 0 & _gridIndex.x < _grid.Dimensions.x)
            {
                // Set tile current
                _tilemap.SetTile(_gridIndex, tile);
                _gridIndex.x++;
            }
        }
        else
        {
            if (_gridIndex.x > 0 & _gridIndex.x <= _grid.Dimensions.x)
            {
                // Delete previous tile
                _gridIndex.x--;
                _tilemap.SetTile(_gridIndex, null);
            }
        }
    }

    public void CheckIfTextIsValid()
    {
        if (!_isEnabled) { return; }
        if (_gridIndex.y > -_grid.Dimensions.y)
        {
            // If there are 5 letters
            if (_gridIndex.x == _grid.Dimensions.x)
            {
                _gridIndex.y--;
                if (_gridIndex.y <= -_grid.Dimensions.y)
                {
                    // Prevent further typing
                    _isEnabled = false;
                }
                _gridIndex.x = 0;
            }
        }
    }

    private Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;
}
