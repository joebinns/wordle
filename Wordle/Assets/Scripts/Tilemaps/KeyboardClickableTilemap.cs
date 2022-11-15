using System.Collections;
using System.Collections.Generic;
using Tilemaps;
using UnityEngine;

public class KeyboardClickableTilemap : ClickableTilemap
{
    [SerializeField] private TextEditor _textEditor;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private LetterTilemapTracker _letterTilemapTracker;
    
    protected override void Click(Vector3 worldPosition)
    {
        var cell = Tilemap.WorldToCell(worldPosition);
        ClickVisual(cell);
        ClickFunctionality(cell);
    }
    
    private void ClickVisual(Vector3Int position)
    {
        TilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
        TilemapAnimator.FlashTileColor(position, Color.white, HoverableTilemap.HoverColor, 0.1f);
        _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
    }

    private void ClickFunctionality(Vector3Int position)
    {
        var name = _letterTilemapTracker.PositionToTileName(position);
        if (name != null)
        {
            var character = name[0];
            _textEditor.SetCharacterAtCaret(character);
        }
    }
}
