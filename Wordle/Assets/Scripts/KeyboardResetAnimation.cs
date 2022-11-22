using System.Collections;
using System.Collections.Generic;
using Tilemaps;
using UnityEngine;

public class KeyboardResetAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private DecoratorTilemapHandler _decoratorTilemapHandler;

    public override IEnumerator Play(Context context)
    {
        var duration = 0.3f;

        var positions = new List<Vector3Int>();

        var bounds = _decoratorTilemapHandler.Tilemap.cellBounds.size;
        for (int x = 0; x < bounds.x; x++)
        {
            for (int y = 0; y < bounds.y; y++)
            {
                var position = new Vector3Int(x, -y, 0);
                if (_decoratorTilemapHandler.Tilemap.HasTile(position))
                {
                    position.y *= -1;
                    positions.Add(position);
                }
            }
        }

        positions.Sort((a, b) => (a.x + a.y).CompareTo(b.x + b.y));

        var magnitude = 0;
        var previousMagnitude = magnitude;
        for (int i = 0; i < positions.Count; i++)
        {
            var position = positions[i];
            magnitude = position.x + position.y;
            position.y *= -1;
            if (magnitude != previousMagnitude)
            {
                yield return new WaitForSeconds(duration / 2f);
                previousMagnitude = magnitude;
            }

            _decoratorTilemapAnimator.SetTileDelayed(position, _decoratorTilemapHandler.TileStateToTile(TileState.UnGuessed), duration / 2f);
            _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
            _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
        }
    }
}
