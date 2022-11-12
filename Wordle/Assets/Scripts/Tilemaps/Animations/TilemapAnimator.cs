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

    public void SetColor(Vector3Int position, Color color)
    {
        if (!_tilemap.HasTile(position)) { return; }
        _tilemap.SetTileFlags(position, TileFlags.None);
        _tilemap.SetColor(position, color);
        _tilemap.SetTileFlags(position, TileFlags.LockColor);
    }

    public void SmoothLoopTilePosition(Vector3Int position)
    {
        StartCoroutine(SmoothLoopTilePositionOnceCoroutine(position, Vector3.zero, Vector3.down * 0.2f));
    }
    
    private IEnumerator SmoothLoopTilePositionOnceCoroutine(Vector3Int position, Vector3 a, Vector3 b)
    {
        yield return StartCoroutine(SmoothTranslateTilePosition(position, a, b));
        StartCoroutine(SmoothTranslateTilePosition(position, b, a));
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
