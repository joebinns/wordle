using System.Collections.Generic;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextEditor : MonoBehaviour
{
    private bool _isEnabled = true;
    public Vector3Int CaretPosition = Vector3Int.zero;
    private List<string> _entries = new List<string>();
    
    [SerializeField] private BlockTilemapHandler _guessesBlockTilemapHandler;
    [SerializeField] private BlockTilemapHandler _keyboardBlockTilemapHandler;
    [SerializeField] private LetterTilemapHandler _guessesLetterTilemapHandler;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;
    private WordChecker _wordChecker;

    public float WordLength => _guessesBlockTilemapHandler.Tilemap.size.x;

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
            if (!_guessesBlockTilemapHandler.Tilemap.HasTile(CaretPosition)) { return; }
            
            // Set current tile
            _guessesLetterTilemapHandler.Tilemap.SetTile(CaretPosition, tile);
            CaretPosition.x++;
        }
        else
        {
            if (!_guessesBlockTilemapHandler.Tilemap.HasTile(CaretPosition + Vector3Int.left)) { return; }

            // Delete previous tile
            CaretPosition.x--;
            _guessesLetterTilemapHandler.Tilemap.SetTile(CaretPosition, null);
        }
    }

    private void SetTileStates(string word, ref Dictionary<int, TileState> indexToTileState)
    {
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
    }

    private void UndoExcessiveTileStates(string word, ref Dictionary<int, TileState> indexToTileState)
    {
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
    }

    private void ApplyTileStates(string word, Dictionary<int, TileState> indexToTileState)
    {
        var position = new Vector3Int(0, CaretPosition.y, 0);
        
        for (int x = 0; x < word.Length; x++)
        {
            position.x = x;
            var character = word[x];
            var tileState = indexToTileState[x];
            _guessesBlockTilemapHandler.SetTileState(position, tileState);
            var keyboardPosition = _keyboardLetterTilemapTracker.TileNameToPosition(character.ToString());
            _keyboardBlockTilemapHandler.SetTileStateCautious(keyboardPosition, tileState);
        }
    }

    private bool CheckIfEntryIsValid()
    {
        var isWordValid = false;
        var isLineFull = (!_guessesBlockTilemapHandler.Tilemap.HasTile(CaretPosition));
        if (isLineFull)
        {
            var word = GetWord(CaretPosition.y);
            var isWordRecognised = _wordChecker.IsWordRecognised(word);
            if (isWordRecognised)
            {
                var isWordAlreadyEntered = _entries.Contains(word);
                if (!isWordAlreadyEntered)
                {
                    isWordValid = true;
                }
            }
        }
        return isWordValid;
    }

    public void EnterText()
    {
        if (!_isEnabled) { return; }

        bool isEntryValid = CheckIfEntryIsValid();
        if (!isEntryValid)
        {
            FindObjectOfType<GuessesAnimations>().HighlightEmptyTiles();
            return;
        }

        var word = GetWord(CaretPosition.y);
        var indexToTileState = new Dictionary<int, TileState>();
        
        // Set tile states based on guess
        SetTileStates(word, ref indexToTileState);

        // Undo excessive tile states (i.e. in cases of duplicate characters)
        UndoExcessiveTileStates(word, ref indexToTileState);

        // Apply tiles based on tile states
        ApplyTileStates(word, indexToTileState);

        // Next line
        NextRow();
        _entries.Add(word);
        
        // Reset game if correct
        var isWordCorrect = _wordChecker.DoesWordMatch(word);
        if (isWordCorrect)
        {
            TriggerReset();
        }
    }

    private void NextRow()
    {
        CaretPosition.x = 0;
        CaretPosition.y--;
        if (!_guessesBlockTilemapHandler.Tilemap.HasTile(CaretPosition))
        {
            TriggerReset();
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

    private void TriggerReset()
    {
        _isEnabled = false;
        GameManager.Instance.ResetGame();
    }

    private void Reset()
    {
        ResetCaret();
        _isEnabled = true;
        _entries.Clear();
    }
        
    private void ResetCaret()
    {
        CaretPosition = Vector3Int.zero;
    }
}
