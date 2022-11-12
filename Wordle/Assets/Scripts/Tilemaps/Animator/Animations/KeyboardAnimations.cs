using Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _blockTilemapAnimator;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;

    [SerializeField] private Color _hoverColor;
    
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
    
    public void PressTile(char character)
    {
        var name = character.ToString();
        if (_keyboardLetterTilemapTracker.Contains(name))
        {
            var position = _keyboardLetterTilemapTracker.TileNameToPosition(name);
            _letterTilemapAnimator.SmoothLoopTilePositionOnce(position);
            _blockTilemapAnimator.SmoothLoopTilePositionOnce(position);
            _blockTilemapAnimator.FlashTileColor(position, Color.white, _hoverColor, 0.05f);
        }
    }

    private void HoverTile(Vector3Int position)
    {
        _blockTilemapAnimator.SetColor(position, _hoverColor);
    }

    private void UnHoverTile(Vector3Int position)
    {
        _blockTilemapAnimator.SetColor(position, Color.white);
    }
}
