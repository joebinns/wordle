using System;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GuessesAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _blockTilemapAnimator;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private LetterTilemapHandler _letterTilemapHandler;

    [SerializeField] private Tile _default;
    [SerializeField] private Tile _select;
    
    private TextEditor _textEditor;
    
    private Vector3Int _hoveredPosition = -Vector3Int.one;
    public Vector3Int HoveredPosition
    {
        get => _hoveredPosition;
        set
        {
            if (_hoveredPosition != value)
            {
                UnHoverTile(_hoveredPosition);
                _hoveredPosition = value;
                HoverTile(_hoveredPosition);
            }
        }
    }

    private void Awake()
    {
        _textEditor = FindObjectOfType<TextEditor>();
    }

    public void HighlightEmptyTiles()
    {
        var position = _textEditor.CaretPosition;
        
        for (int x = 0; x < _textEditor.WordLength; x++)
        {
            position.x = x;

            if (_letterTilemapHandler.Tilemap.HasTile(position)) { continue; }
            
            var flashDuration = 0.5f;
            _blockTilemapAnimator.FlashTile(position, _default, _select, flashDuration * 1.2f);
            _blockTilemapAnimator.SmoothFlipTileOnce(position, flashDuration);
            _letterTilemapAnimator.SmoothFlipTileOnce(position, flashDuration, true);
        }
    }
    
    public void PressTile(char character)
    {
        /*
        var name = character.ToString();
        if (_keyboardLetterTilemapTracker.Contains(name))
        {
            var position = _keyboardLetterTilemapTracker.TileNameToPosition(name);
            _tilemapAnimator.SmoothLoopTilePosition(position);
        }
        */
    }

    private void HoverTile(Vector3Int position)
    {
        var color = Color.white;
        color.a = 0.8f;
        _blockTilemapAnimator.SetColor(position, color);
    }

    private void UnHoverTile(Vector3Int position)
    {
        var color = Color.white;
        _blockTilemapAnimator.SetColor(position, color);
    }
}
