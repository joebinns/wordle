using Tilemaps;
using UnityEngine;

public class KeyboardAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _tilemapAnimator;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;
    
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
            _tilemapAnimator.SmoothLoopTilePosition(position);
        }
    }

    private void HoverTile(Vector3Int position)
    {
        var color = Color.white;
        color.a = 0.8f;
        _tilemapAnimator.SetColor(position, color);
    }

    private void UnHoverTile(Vector3Int position)
    {
        var color = Color.white;
        _tilemapAnimator.SetColor(position, color);
    }
}
