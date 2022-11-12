using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapAnimator : MonoBehaviour
{
    // TODO: Move color overlay stuff into this script.
    
    private Tilemap _tilemap;
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

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

    public void SmoothLoopTilePosition(Vector3Int tile)
    {
        StartCoroutine(SmoothLoopTilePositionOnceCoroutine(tile, Vector3.zero, Vector3.down * 0.15f));
    }
    
    private IEnumerator SmoothLoopTilePositionOnceCoroutine(Vector3Int tile, Vector3 a, Vector3 b)
    {
        yield return StartCoroutine(SmoothTranslateTilePosition(tile, a, b));
        StartCoroutine(SmoothTranslateTilePosition(tile, b, a));
    }
    
    // TODO: Also lerp the letters on the keys.
    private IEnumerator SmoothTranslateTilePosition(Vector3Int tile, Vector3 start, Vector3 end)
    {
        //HoverTile(position); // TODO: Move this to an appropriate place
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
        //UnHoverTile(position);
    }
}
