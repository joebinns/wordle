using Tilemaps;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private TextEditor _textEditor;
    [SerializeField] private LetterTilemapTracker  _keyboardLetterTilemapTracker;
    [SerializeField] private BlockTilemapHandler _keyboardBlockTilemapHandler;

    private KeyboardAnimations _keyboardAnimations;

    private void Awake()
    {
        _keyboardAnimations = FindObjectOfType<KeyboardAnimations>();
    }

    private void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {
            var tilemapPosition = _keyboardBlockTilemapHandler.Tilemap.WorldToCell(rayHit.point);
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
        else
        {
            // Un-hover (Note: There should never be a tile at this position).
            _keyboardAnimations.HoveredPosition = -Vector3Int.one;
        }
    }
}
