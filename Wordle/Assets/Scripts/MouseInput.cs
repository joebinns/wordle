using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseInput : MonoBehaviour
{
    private TextTilemap _textTilemap;
    private KeyboardTilemap _keyboardTilemap;
    private KeyboardTileTilemap _keyboardTileTilemap;

    private Vector3Int _hoveredPosition;
    public Vector3Int HoveredPosition
    {
        get => _hoveredPosition;
        set
        {
            if (_hoveredPosition != value)
            {
                StartCoroutine(UnHoverTile(_hoveredPosition));
                _hoveredPosition = value;
                StartCoroutine(HoverTile(_hoveredPosition));
            }
        }
    }

    private void Awake()
    {
        _textTilemap = FindObjectOfType<TextTilemap>();
        _keyboardTilemap = FindObjectOfType<KeyboardTilemap>();
        _keyboardTileTilemap = FindObjectOfType<KeyboardTileTilemap>();
    }

    private void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {
            var tilemapPosition = _keyboardTileTilemap.Tilemap.WorldToCell(rayHit.point);
            var name = _keyboardTilemap.PositionToTileName(tilemapPosition);

            HoveredPosition = tilemapPosition;
            
            if (name != null)
            {
                var character = name[0];
                if (Input.GetMouseButtonDown(0))
                {
                    _textTilemap.SetCharacterAtCaret(character);
                }
            }
        }
    }

    private IEnumerator HoverTile(Vector3Int position)
    {
        yield return StartCoroutine(LerpTileScale(position, 1.0f, 1.1f));
    }

    private IEnumerator UnHoverTile(Vector3Int position)
    {
        yield return StartCoroutine(LerpTileScale(position, 1.1f, 1.0f));
    }
    
    private IEnumerator LerpTileScale(Vector3Int position, float start, float end)
    {
        var tilemap = _keyboardTileTilemap.Tilemap;
        tilemap.SetTileFlags(position, TileFlags.None);
        var t = 0f;
        var duration = 0.1f;
        while (t < duration)
        {
            var magnitude = start + ((end - start) * t);
            Matrix4x4 newMatrix = Matrix4x4.Scale(Vector3.one * magnitude);
            tilemap.SetTransformMatrix(position, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = Matrix4x4.Scale(Vector3.one * end);
        tilemap.SetTransformMatrix(position, targetMatrix);
    }
}
