using System.Collections.Generic;
using System.Linq;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GuessesAnimationsController : MonoBehaviour
{
    [SerializeField] private DecoratorTilemapHandler _guessesDecoratorTilemapHandler;
    
    private GuessesAnimations _guessesAnimations;
    private WordleTextEditor _wordleTextEditor;
    private WordChecker _wordChecker;

    private void Awake()
    {
        _guessesAnimations = GetComponent<GuessesAnimations>();
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    private void OnEnable()
    {
        WordleTextEditor.OnInvalidInput += Shake;
        WordleTextEditor.OnTextChanged += OnTextChanged;
    }
    
    private void OnDisable()
    {
        WordleTextEditor.OnInvalidInput -= Shake;
        WordleTextEditor.OnTextChanged -= OnTextChanged;
    }

    private void OnTextChanged(char character)
    {
        var characterIndex = _wordleTextEditor.GetFinalLine().Length - 1;
        var characterPosition = IndexToPosition(characterIndex);

        if (character is >= 'a' and <= 'z')
        {
            var tile = TilemapUtilities.FindTileByCharacter(character);
            _guessesAnimations.SetLetter(characterPosition, tile);
        }
        else if (character == '\b')
        {
            characterPosition.x++;
            _guessesAnimations.SetLetter(characterPosition, null);
        }
        else if (character == '\r')
        {
            var word = _wordleTextEditor.GetLine(_wordleTextEditor.GetFinalLineIndex() - 1);
            var indexToTileState = _wordChecker.GetTileStates(word);
            var indices = indexToTileState.Keys.ToList();
            var positionToTile = new Dictionary<Vector3Int, Tile>();
            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                var position = IndexToPosition(index);
                position.y++;
                var tile = _guessesDecoratorTilemapHandler.TileStateToTile(indexToTileState[index]);
                positionToTile[position] = tile;
            }
            _guessesAnimations.RevealGuessTiles(positionToTile);
        }
    }
    
    private void Shake()
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        
        var fullIndices = _wordleTextEditor.GetFullIndices(lineIndex);
        var fullPositions = IndicesToPositions(fullIndices);
        
        var emptyIndices = _wordleTextEditor.GetEmptyIndices(lineIndex);
        var emptyPositions = IndicesToPositions(emptyIndices);

        var areAllPositionsFull = emptyPositions.Count == 0;

        _guessesAnimations.HighlightTiles(areAllPositionsFull ? fullPositions : emptyPositions);
        _guessesAnimations.ShakeTilesReactive(areAllPositionsFull ? fullPositions : emptyPositions, areAllPositionsFull ? emptyPositions : fullPositions, Vector3.zero, areAllPositionsFull ? Vector3.left : Vector3.right);
    }

    private Vector3Int IndexToPosition(int index)
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        return new Vector3Int(index, -lineIndex, 0);
    }

    private List<Vector3Int> IndicesToPositions(List<int> indices)
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        return indices.Select(characterIndex => new Vector3Int(characterIndex, -lineIndex, 0)).ToList();
    }
}
