using Tilemaps;
using UnityEngine;

public class KeyboardClickableTilemap : ClickableTilemap
{
    [SerializeField] private TextEditor _textEditor;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private LetterTilemapTracker _letterTilemapTracker;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        KeyboardInput.OnKeyDown += Press;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        KeyboardInput.OnKeyDown -= Press;
    }

    // Click (world) --> convert to cell --> visual (cell) --> convert to char --> functionality (char).
    protected override void Click(Vector3 worldPosition)
    {
        var cell = Tilemap.WorldToCell(worldPosition);
        if (!Tilemap.HasTile(cell)) { return; }
        ClickVisual(cell);
        var character = _letterTilemapTracker.PositionToCharacter(cell);
        ClickFunctionality(character);
    }
    
    // Press (char) --> convert to cell --> visual (cell) --> functionality (char).
    private void Press(char character)
    {
        if (!_letterTilemapTracker.Contains(character)) { return; }
        var cell = _letterTilemapTracker.CharacterToPosition(character);
        ClickVisual(cell);
        ClickFunctionality(character);
    }

    private void ClickVisual(Vector3Int position)
    {
        TilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
        TilemapAnimator.FlashTileColor(position, Color.white, HoverableTilemap.HoverColor, 0.1f);
        _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
    }

    private void ClickFunctionality(char character)
    {
        _textEditor.InterpretCharacter(character);
    }
}
