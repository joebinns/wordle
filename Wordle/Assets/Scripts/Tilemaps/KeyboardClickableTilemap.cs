using Tilemaps;
using UnityEngine;

public class KeyboardClickableTilemap : ClickableTilemap
{
    [SerializeField] private WordleAnimationsController _WordleAnimationsController;
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
        var character = _textTilemapTracker.PositionToCharacter(cell);
        ClickVisual(character);
        ClickFunctionality(character);
    }

    // Press (char) --> convert to cell --> visual (cell) --> functionality (char).
    private void Press(char character)
    {
        if (!_textTilemapTracker.Contains(character)) { return; }
        ClickVisual(character);
        ClickFunctionality(character);
    }

    private void ClickVisual(char character)
    {
        _WordleAnimationsController.PlayClickAnimation(character);
    }

    private void ClickFunctionality(char character)
    {
        if (!_wordleTextEditor.IsEnabled) { GameManager.Instance.ResetGame(0f); return; }
        _wordleTextEditor.AmendText(character);
    }
}
