using System.Collections;
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
    public void SetTileDelayed(Vector3Int position, Tile tile, float delay)
    {
        StartCoroutine(SetTileDelayedCoroutine(position, tile, delay));
    }

    private IEnumerator SetTileDelayedCoroutine(Vector3Int position, Tile tile, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetTile(position, tile);
    }
    
    public void SetTile(Vector3Int position, Tile tile)
    {
        _tilemap.SetTile(position, tile);
    }
    
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

    public void SetRotation(Vector3Int tile, Vector3 rotation)
    {
        Matrix4x4 targetMatrix = Matrix4x4.Rotate(Quaternion.Euler(rotation));
        _tilemap.SetTransformMatrix(tile, targetMatrix);
    }
    
    public void SmoothTrickHalfFlipTileOnce(Vector3Int tile, float duration, bool opposite = false)
    {
        var a = Vector3.zero;
        var b = Vector3.right * 90f;
        StartCoroutine(SmoothTrickHalfFlipTileOnceCoroutine(tile, opposite ? b : a, opposite ? a : b, duration));
    }

    private IEnumerator SmoothTrickHalfFlipTileOnceCoroutine(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        duration /= 2f;
        yield return StartCoroutine(SmoothRotateTile(tile, a, b, duration));
        StartCoroutine(SmoothRotateTile(tile, -b, a, duration));
    }
    
    public void SmoothHalfFlipTileOnce(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        StartCoroutine(SmoothRotateTile(tile, a, b, duration));
    }

    public void OscillateHalfFlipTileOnce(Vector3Int tile, float duration)
    {
        var axis = new Ray(Vector3.up * 0.5f, Vector3.right); // TODO: Replace origin with real tile size
        var a = 270f;
        var b = 360f;
        StartCoroutine(SmoothRotateTileAroundAxis(tile, a, b, axis, duration));
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
    
    private IEnumerator SmoothRotateTileAroundAxis(Vector3Int tile, float start, float end, Ray axis, float duration)
    {
        _tilemap.SetTileFlags(tile, TileFlags.None);
        
        var t = 0f;
        while (t < duration)
        {
            var progress = t / duration;
            var rotation = start + ((end - start) * progress);
            Matrix4x4 newMatrix = RotationMatrixAroundAxis(axis, rotation);
            _tilemap.SetTransformMatrix(tile, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = RotationMatrixAroundAxis(axis, end);
        _tilemap.SetTransformMatrix(tile, targetMatrix);
    }
    
    public Matrix4x4 RotationMatrixAroundAxis(Ray axis, float rotation)
    {
        /*
        return Matrix4x4.TRS(-axis.origin, Quaternion.AngleAxis(rotation, axis.direction), Vector3.one)
               * Matrix4x4.TRS(axis.origin, Quaternion.identity, Vector3.one);
        */
        return Matrix4x4.TRS(axis.origin, Quaternion.AngleAxis(rotation, axis.direction), Vector3.one)
               * Matrix4x4.TRS(axis.origin, Quaternion.identity, Vector3.one);
    }
    #endregion

    #region Translate

    public void SmoothShakeTileOnce(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        StartCoroutine(SmoothShakeTileOnceCoroutine(tile, a, b, duration));
    }

    private IEnumerator SmoothShakeTileOnceCoroutine(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        duration /= 2f;
        yield return StartCoroutine(SmoothLoopTilePositionOnceCoroutine(tile, a, b, duration));
        yield return StartCoroutine(SmoothLoopTilePositionOnceCoroutine(tile, a, -b, duration));
    }
    
    public void SmoothLoopTilePositionOnce(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        StartCoroutine(SmoothLoopTilePositionOnceCoroutine(tile, a, b, duration));
    }
    
    private IEnumerator SmoothLoopTilePositionOnceCoroutine(Vector3Int tile, Vector3 a, Vector3 b, float duration)
    {
        duration /= 2f;
        yield return StartCoroutine(SmoothTranslateTile(tile, a, b, duration));
        yield return StartCoroutine(SmoothTranslateTile(tile, b, a, duration));
    }
    
    private IEnumerator SmoothTranslateTile(Vector3Int tile, Vector3 start, Vector3 end, float duration)
    {
        _tilemap.SetTileFlags(tile, TileFlags.None);
        
        var t = 0f;
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
