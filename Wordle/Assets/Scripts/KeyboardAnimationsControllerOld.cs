using System.Collections.Generic;
using System.Linq;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardAnimationsControllerOld : MonoBehaviour
{
    [SerializeField] private DecoratorTilemapHandler _keyboardDecoratorTilemapHandler;
    [SerializeField] private TextTilemapTracker _textTilemapTracker;
    
    private KeyboardAnimations _keyboardAnimations;
    private WordleTextEditor _wordleTextEditor;
    private WordChecker _wordChecker;

    private void Awake()
    {
        _keyboardAnimations = GetComponent<KeyboardAnimations>();
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    private void OnEnable()
    {
        WordleTextEditor.OnTextChanged += OnTextChanged;
    }
    
    private void OnDisable()
    {
        WordleTextEditor.OnTextChanged -= OnTextChanged;
    }

    public void ClickTile(char character)
    {
        var position = _textTilemapTracker.CharacterToPosition(character);
        _keyboardAnimations.ClickTile(position);
    }

    private void OnTextChanged(char input)
    {
        if (input == '\r')
        {
            var word = _wordleTextEditor.GetLine(_wordleTextEditor.GetFinalLineIndex() - 1);
            var indexToTileState = _wordChecker.GetTileStates(word);
            var indices = indexToTileState.Keys.ToList();
            var positionToTile = new Dictionary<Vector3Int, Tile>();
            for (int i = 0; i < indices.Count; i++)
            {
                var character = word[i];
                var index = indices[i];
                var tileState = indexToTileState[index];
                var characterPosition = _textTilemapTracker.CharacterToPosition(character);
                if (tileState > _keyboardDecoratorTilemapHandler.PositionToTileState[characterPosition])
                {
                    var tile = _keyboardDecoratorTilemapHandler.TileStateToTile(tileState);
                    positionToTile[characterPosition] = tile;
                }
            }
            _keyboardAnimations.RevealGuessTiles(positionToTile);
        }
    }
}
