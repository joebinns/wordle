using System.Collections;
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
                //StartCoroutine(UnHoverTile(_hoveredPosition));
                UnHoverTile(_hoveredPosition);
                _hoveredPosition = value;
                //StartCoroutine(HoverTile(_hoveredPosition));
                HoverTile(_hoveredPosition);
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
                    PressTile(character);
                }
            }
        }
        else
        {
            // Un-hover (Note: There should never be a tile at this position).
            HoveredPosition = -Vector3Int.one;
        }
    }

    public void PressTile(char character)
    {
        var name = character.ToString();
        if (_keyboardTilemap.TileNameToPosition.ContainsKey(name))
        {
            var position = _keyboardTilemap.TileNameToPosition[name];
            StartCoroutine(PressTileCoroutine(position));
        }
    }
    
    // TODO: Add button press (darken button then lighten)?
    private IEnumerator PressTileCoroutine(Vector3Int position) // Call this from KeyboardInput.
    {
        yield return StartCoroutine(LerpTilePosition(position, 0, -0.2f));
        yield return StartCoroutine(LerpTilePosition(position, -0.2f, 0));
    }

    private void HoverTile(Vector3Int position)
    {
        var colorOverlay = Color.white;
        colorOverlay.a = 0.8f;
        _keyboardTileTilemap.ApplyColorOverlay(position, colorOverlay);
    }

    private void UnHoverTile(Vector3Int position)
    {
        _keyboardTileTilemap.ResetColor(position);
    }

    // TODO: Also lerp the letters on the keys.
    private IEnumerator LerpTilePosition(Vector3Int position, float start, float end)
    {
        HoverTile(position);
        var tilemap = _keyboardTileTilemap.Tilemap;
        tilemap.SetTileFlags(position, TileFlags.None);
        var t = 0f;
        var duration = 0.05f;
        while (t < duration)
        {
            // TODO: Convert lerp to eased function OR curve
            var magnitude = start + ((end - start) * t);
            Matrix4x4 newMatrix = Matrix4x4.Translate(Vector3.up * magnitude);
            tilemap.SetTransformMatrix(position, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = Matrix4x4.Translate(Vector3.one * end);
        tilemap.SetTransformMatrix(position, targetMatrix);
        UnHoverTile(position);
    }
}
