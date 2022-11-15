using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapAnimator))]
[RequireComponent(typeof(HoverableTilemap))]
public class ClickableTilemap : MonoBehaviour
{
    private Tilemap _tilemap;
    private TilemapAnimator _tilemapAnimator;
    private HoverableTilemap _hoverableTilemap;
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _tilemapAnimator = GetComponent<TilemapAnimator>();
        _hoverableTilemap = GetComponent<HoverableTilemap>();
    }

    private void OnEnable()
    {
        MouseInput.OnCursorClick += Click;
    }
    
    private void OnDisable()
    {
        MouseInput.OnCursorClick -= Click;
    }

    private void Click(Vector3 position)
    {
        var cell = _tilemap.WorldToCell(position);
        ClickVisual(cell);
        ClickFunctionality(cell);
    }

    private void ClickVisual(Vector3Int position)
    {
        _tilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
        _tilemapAnimator.FlashTileColor(position, Color.white, _hoverableTilemap.HoverColor, 0.1f);
    }

    private void ClickFunctionality(Vector3Int position)
    {
        // TODO: How else can I access PositionToTileName...?
        // TODO: How else can I access the text editor...? Should I even have text editor?
        /*
        var name = _keyboardLetterTilemapTracker.PositionToTileName(position);
        if (name != null)
        {
            var character = name[0];
            _textEditor.SetCharacterAtCaret(character);
            _keyboardAnimations.PressTile(character);
        }
        */
    }
}