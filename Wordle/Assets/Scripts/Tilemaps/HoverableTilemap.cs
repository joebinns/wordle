using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapAnimator))]
public class HoverableTilemap : MonoBehaviour
{
    private Color _hoverColor = new Color(1f, 1f, 1f, 0.5f);
    public Color HoverColor => _hoverColor;
    
    private Tilemap _tilemap;
    private TilemapAnimator _tilemapAnimator;

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
        _tilemap = GetComponent<Tilemap>();
        _tilemapAnimator = GetComponent<TilemapAnimator>();
    }

    private void OnEnable()
    {
        MouseInput.OnCursorPositionChanged += SetHoverPosition;
    }
    
    private void OnDisable()
    {
        MouseInput.OnCursorPositionChanged -= SetHoverPosition;
    }

    private void SetHoverPosition(Vector3 position)
    {
        HoveredPosition = _tilemap.WorldToCell(position);
    }

    private void HoverTile(Vector3Int position)
    {
        _tilemapAnimator.SetColor(position, _hoverColor);
    }

    private void UnHoverTile(Vector3Int position)
    {
        _tilemapAnimator.SetColor(position, Color.white);
    }
}
