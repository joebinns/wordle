using Tilemaps;
using UnityEngine;

public class KeyboardClickableTilemap : ClickableTilemap
{
    [SerializeField] private KeyboardAnimations _keyboardAnimations;
    [SerializeField] private TextTilemapTracker _textTilemapTracker;

    private WordleTextEditor _wordleTextEditor;
    
    protected override void Awake()
    {
        base.Awake();
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

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
        var character = _textTilemapTracker.PositionToCharacter(cell);
        ClickFunctionality(character);
    }
    
    // Press (char) --> convert to cell --> visual (cell) --> functionality (char).
    private void Press(char character)
    {
        if (!_textTilemapTracker.Contains(character)) { return; }
        var cell = _textTilemapTracker.CharacterToPosition(character);
        ClickVisual(cell);
        ClickFunctionality(character);
    }

    private void ClickVisual(Vector3Int position)
    {
        _keyboardAnimations.ClickTile(position);
    }

    private void ClickFunctionality(char character)
    {
        _wordleTextEditor.AmendText(character);
    }
}
