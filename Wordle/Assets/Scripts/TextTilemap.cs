using UnityEngine;
using UnityEngine.Tilemaps;

public class TextTilemap : MonoBehaviour
{
    private Vector3Int _gridIndex = Vector3Int.zero;
    [SerializeField] private Tile _tile;
    private bool _isEnabled = true;

    private Grid _grid;
    private Tilemap _tilemap;
    private WordChecker _wordChecker;
    private TileTilemap _tileTilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _grid = FindObjectOfType<Grid>();
        _wordChecker = FindObjectOfType<WordChecker>();
        _tileTilemap = FindObjectOfType<TileTilemap>();
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

    public void EnterText()
    {
        if (!_isEnabled) { return; }
        if (_gridIndex.y > -_grid.Dimensions.y)
        {
            if (_gridIndex.x == _grid.Dimensions.x)
            {
                var word = GetWord(_gridIndex.y);
                if (_wordChecker.IsWordRecognised(word))
                {
                    // TODO: Check which letters are included, and their positions.
                    var position = new Vector3Int(0, _gridIndex.y, 0);
                    for (int x = 0; x < word.Length; x++)
                    {
                        position.x = x;
                        var character = word[x];
                        var tileState = TileState.WrongGuess;
                        if (_wordChecker.IsCharacterIncluded(character))
                        {
                            tileState = TileState.SemiCorrectGuess;
                            if (_wordChecker.IsCharacterIncludedAtIndex(character, x))
                            {
                                tileState = TileState.CorrectGuess;
                            }
                        }
                        _tileTilemap.SetTile(position, tileState);
                    }
                    NextRow();
                }
            }
        }
    }

    private void NextRow()
    {
        _gridIndex.y--;
        if (_gridIndex.y <= -_grid.Dimensions.y)
        {
            // Prevent further typing
            _isEnabled = false;
        }
        _gridIndex.x = 0;
    }

    private Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;
    
    private string GetWord(int yPosition)
    {
        string word = "";
        for (int x = 0; x < _grid.Dimensions.x; x++)
        {
            var coord = new Vector3Int(x, yPosition, 0);
            word += _tilemap.GetTile(coord).name;
        }
        return (word);
    }
}
