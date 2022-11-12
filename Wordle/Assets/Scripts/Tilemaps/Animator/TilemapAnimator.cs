using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapAnimator : MonoBehaviour
{
    private Tilemap _tilemap;
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    #region Tile
    public void FlashTile(Vector3Int tile, Tile a, Tile b, float duration)
    {
        StartCoroutine(FlashTileCoroutine(tile, a, b, duration));
    }

    private IEnumerator FlashTileCoroutine(Vector3Int tile, Tile a, Tile b, float duration)
    {
        _tilemap.SetTile(tile, b);
        yield return new WaitForSeconds(duration);
        _tilemap.SetTile(tile, a);
    }
    #endregion

    #region Color
    public void FlashTileColor(Vector3Int tile, Color a, Color b, float duration)
    {
        StartCoroutine(FlashTileColorCoroutine(tile, a, b, duration));
    }

    private IEnumerator FlashTileColorCoroutine(Vector3Int tile, Color a, Color b, float duration)
    {
        SetColor(tile, b);
        yield return new WaitForSeconds(duration);
        SetColor(tile, a);
    }
    
    public void SetColor(Vector3Int tile, Color color)
    {
        if (!_tilemap.HasTile(tile)) { return; }
        _tilemap.SetTileFlags(tile, TileFlags.None);
        _tilemap.SetColor(tile, color);
        _tilemap.SetTileFlags(tile, TileFlags.LockColor);
    }
    #endregion
    
    #region Rotate
    public void SmoothFlipTileOnce(Vector3Int tile, float duration, bool opposite = false)
    {
        var a = Vector3.zero;
        var b = Vector3.right * 180f;
        StartCoroutine(SmoothRotateTile(tile, opposite ? b : a, opposite ? a : b, duration));
    }

    private IEnumerator SmoothRotateTile(Vector3Int tile, Vector3 start, Vector3 end, float duration)
    {
        _tilemap.SetTileFlags(tile, TileFlags.None);
        
        var t = 0f;
        while (t < duration)
        {
            var progress = t / duration;
            var orientation = start + ((end - start) * progress);
            var rotation = Quaternion.Euler(orientation);
            Matrix4x4 newMatrix = Matrix4x4.Rotate(rotation);
            _tilemap.SetTransformMatrix(tile, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = Matrix4x4.Rotate(Quaternion.Euler(end));
        _tilemap.SetTransformMatrix(tile, targetMatrix);
    }
    #endregion

    #region Translate
    public void SmoothLoopTilePositionOnce(Vector3Int tile)
    {
        StartCoroutine(SmoothLoopTilePositionOnceCoroutine(tile, Vector3.zero, Vector3.down * 0.15f));
    }
    
    private IEnumerator SmoothLoopTilePositionOnceCoroutine(Vector3Int tile, Vector3 a, Vector3 b)
    {
        yield return StartCoroutine(SmoothTranslateTile(tile, a, b));
        StartCoroutine(SmoothTranslateTile(tile, b, a));
    }
    
    private IEnumerator SmoothTranslateTile(Vector3Int tile, Vector3 start, Vector3 end)
    {
        _tilemap.SetTileFlags(tile, TileFlags.None);
        var t = 0f;
        var duration = 0.05f; // TODO: Change to curve OR eased lerp
        while (t < duration)
        {
            var progress = t / duration;
            var translation = start + ((end - start) * progress);
            Matrix4x4 newMatrix = Matrix4x4.Translate(translation);
            _tilemap.SetTransformMatrix(tile, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = Matrix4x4.Translate(end);
        _tilemap.SetTransformMatrix(tile, targetMatrix);
    }
    #endregion
}