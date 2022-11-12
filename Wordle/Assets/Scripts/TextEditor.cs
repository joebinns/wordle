using System.Collections.Generic;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextEditor : MonoBehaviour
{
    private bool _isEnabled = true;
    private Vector3Int _caretPosition = Vector3Int.zero;
    private List<string> _entries = new List<string>();
    
    [SerializeField] private BlockTilemapHandler _guessesBlockTilemapHandler;
    [SerializeField] private BlockTilemapHandler _keyboardBlockTilemapHandler;
    [SerializeField] private LetterTilemapHandler _guessesLetterTilemapHandler;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;
    private WordChecker _wordChecker;

    private void Awake()
    {
        _guessesLetterTilemapHandler = FindObjectOfType<LetterTilemapHandler>();
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }
    
    private Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;

    public void SetCharacterAtCaret(char character)
    {
        if (!_isEnabled) { return; }
        var tile = FindTileByCharacter(character);
        if (tile != null)
        {
            if (_guessesBlockTilemapHandler.Tilemap.HasTile(_caretPosition))
            {
                // Set tile current
                _guessesLetterTilemapHandler.Tilemap.SetTile(_caretPosition, tile);
                _caretPosition.x++;
            }
        }
        else
        {
            if (_guessesBlockTilemapHandler.Tilemap.HasTile(_caretPosition + Vector3Int.left))
            {
                // Delete previous tile
                _caretPosition.x--;
                _guessesLetterTilemapHandler.Tilemap.SetTile(_caretPosition, null);
            }
        }
    }

    public void EnterText()
    {
        if (!_isEnabled) { return; }

        if (!_guessesBlockTilemapHandler.Tilemap.HasTile(_caretPosition))
        {
            var word = GetWord(_caretPosition.y);
            if (_wordChecker.IsWordRecognised(word))
            {
                if (!_entries.Contains(word))
                {
                    var position = new Vector3Int(0, _caretPosition.y, 0);
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
                        _guessesBlockTilemapHandler.SetTileState(position, tileState);
                        var keyboardPosition = _keyboardLetterTilemapTracker.TileNameToPosition(character.ToString());
                        _keyboardBlockTilemapHandler.SetTileStateCautious(keyboardPosition, tileState);
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


    private void NextRow()
    {
        _caretPosition.x = 0;
        _caretPosition.y--;
        if (!_guessesBlockTilemapHandler.Tilemap.HasTile(_caretPosition))
        {
            _isEnabled = false;
            GameManager.Instance.ResetGame();
        }
    }
    
    private string GetWord(int yPosition)
    {
        string word = "";
        var coord = new Vector3Int(0, yPosition, 0);
        for (int x = 0; x < _guessesLetterTilemapHandler.Tilemap.size.x; x++)
        {
            coord.x = x;
            word += _guessesLetterTilemapHandler.Tilemap.GetTile(coord).name;
        }
        return (word);
    }

    private void Reset()
    {
        ResetCaret();
        _isEnabled = true;
        _entries.Clear();
    }
        
    private void ResetCaret()
    {
        _caretPosition = Vector3Int.zero;
    }
}
