using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextTilemap : MonoBehaviour
{
    private Vector3Int _gridIndex = Vector3Int.zero;
    private bool _isEnabled = true;

    private Grid _grid;
    private Tilemap _tilemap;
    private WordChecker _wordChecker;
    private TileTilemap _tileTilemap;
    private KeyboardTileTilemap _keyboardTileTilemap;
    private List<string> _entries = new List<string>();

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _grid = FindObjectOfType<Grid>();
        _wordChecker = FindObjectOfType<WordChecker>();
        _tileTilemap = FindObjectOfType<TileTilemap>();
        _keyboardTileTilemap = FindObjectOfType<KeyboardTileTilemap>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
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
                    if (!_entries.Contains(word))
                    { 
                        var position = new Vector3Int(0, _gridIndex.y, 0);
                        var indexToTileState = new Dictionary<int, TileState>();
                        for (int x = 0; x < word.Length; x++)
                        {
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
                            indexToTileState[x] = tileState;
                        }

                        var charToExcess = _wordChecker.GetCharToExcess(word);
                        for (int x = word.Length - 1; x >= 0; x--)
                        {
                            var character = word[x];
                            if (indexToTileState[x] == TileState.SemiCorrectGuess)
                            {
                                if (charToExcess[character] > 0)
                                {
                                    indexToTileState[x] = TileState.WrongGuess;
                                    charToExcess[character] -= 1;
                                }
                            }
                        }
                        
                        for (int x = 0; x < word.Length; x++)
                        {
                            position.x = x;
                            var character = word[x];
                            var tileState = indexToTileState[x];
                            _tileTilemap.SetTile(position, tileState);
                            var keyboardPosition = _keyboardTileTilemap.TileNameToPosition(character.ToString());
                            if (_keyboardTileTilemap.PositionToTileState[keyboardPosition] < tileState)
                            {
                                _keyboardTileTilemap.SetColor(keyboardPosition, tileState);
                            }
                        }

                        NextRow();
                        _entries.Add(word);
                        if (_wordChecker.DoesWordMatch(word))
                        {
                            _isEnabled = false;
                            GameManager.Instance.ResetGame();
                        }
                    }
                }
            }
        }
    }

    private void NextRow()
    {
        _gridIndex.y--;
        if (_gridIndex.y <= -_grid.Dimensions.y)
        {
            _isEnabled = false;
            GameManager.Instance.ResetGame();
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

    private void Reset()
    {
        ResetTilemap();
        ResetCaret();
        _isEnabled = true;
        _entries.Clear();
    }

    private void ResetTilemap()
    {
        _tilemap.ClearAllTiles();
    }

    private void ResetCaret()
    {
        _gridIndex = Vector3Int.zero;
    }
}
