using Tilemaps;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private TextEditor _textEditor;
    [SerializeField] private LetterTilemapTracker  _keyboardLetterTilemapTracker;
    [SerializeField] private BlockTilemapHandler _keyboardBlockTilemapHandler;
    [SerializeField] private BlockTilemapHandler _guessesBlockTilemapHandler;

    private KeyboardAnimations _keyboardAnimations;
    private GuessesAnimations _guessesAnimations;

    private void Awake()
    {
        _keyboardAnimations = FindObjectOfType<KeyboardAnimations>();
        _guessesAnimations = FindObjectOfType<GuessesAnimations>();
    }

    private void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        // TODO: Check if keyboard or guesses. If guesses, set caret position (if same line) on click.
        if (rayHit.collider != null)
        {
            if (rayHit.collider.gameObject == _keyboardBlockTilemapHandler.gameObject)
            {
                HoveringKeyboard(rayHit.point);
            }
            else if (rayHit.collider.gameObject == _guessesBlockTilemapHandler.gameObject)
            {
                HoveringGuesses(rayHit.point);
            }
        }
        else
        {
            // Un-hover (Note: There should never be a tile at this position).
            _keyboardAnimations.HoveredPosition = -Vector3Int.one;
            _guessesAnimations.HoveredPosition = -Vector3Int.one;
        }
    }
    
    private void HoveringGuesses(Vector3 cursorPosition)
    {
        var tilemapPosition = _guessesBlockTilemapHandler.Tilemap.WorldToCell(cursorPosition);
        _guessesAnimations.HoveredPosition = tilemapPosition;
        
        if (Input.GetMouseButtonDown(0))
        {
            // TODO: Set caret position
            //_guessesAnimations.PressTile(tilemapPosition);
        }
    }
    
    private void HoveringKeyboard(Vector3 cursorPosition)
    {
        var tilemapPosition = _keyboardBlockTilemapHandler.Tilemap.WorldToCell(cursorPosition);
        _keyboardAnimations.HoveredPosition = tilemapPosition;

        var name = _keyboardLetterTilemapTracker.PositionToTileName(tilemapPosition);
        if (name != null)
        {
            var character = name[0];
            if (Input.GetMouseButtonDown(0))
            {
                _textEditor.SetCharacterAtCaret(character);
                _keyboardAnimations.PressTile(character);
            }
        }
    }
}
