using System.Collections;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseInput : MonoBehaviour
{
    [SerializeField] private TextEditor _textEditor;
    [SerializeField] private LetterTilemapTracker  _keyboardLetterTilemapTracker;
    [SerializeField] private BlockTilemapHandler _keyboardBlockTilemapHandler;

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

    private void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {
            var tilemapPosition = _keyboardBlockTilemapHandler.Tilemap.WorldToCell(rayHit.point);
            var name = _keyboardLetterTilemapTracker.PositionToTileName(tilemapPosition);

            HoveredPosition = tilemapPosition;
            
            if (name != null)
            {
                var character = name[0];
                if (Input.GetMouseButtonDown(0))
                {
                    _textEditor.SetCharacterAtCaret(character);
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
        if (_keyboardLetterTilemapTracker.Contains(name))
        {
            var position = _keyboardLetterTilemapTracker.TileNameToPosition(name);
            StartCoroutine(PressTileCoroutine(position));
        }
    }
    
    private IEnumerator PressTileCoroutine(Vector3Int position) // Also call this from KeyboardInput.
    {
        yield return StartCoroutine(LerpTilePosition(position, 0, -0.2f));
        yield return StartCoroutine(LerpTilePosition(position, -0.2f, 0));
    }

    private void HoverTile(Vector3Int position)
    {
        var colorOverlay = Color.white;
        colorOverlay.a = 0.8f;
        _keyboardBlockTilemapHandler.ApplyColorOverlay(position, colorOverlay);
    }

    private void UnHoverTile(Vector3Int position)
    {
        _keyboardBlockTilemapHandler.ResetColorOverlay(position);
    }

    // TODO: Also lerp the letters on the keys.
    private IEnumerator LerpTilePosition(Vector3Int position, float start, float end)
    {
        HoverTile(position);
        var tilemap = _keyboardBlockTilemapHandler.Tilemap;
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
