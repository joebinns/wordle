using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResetAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private Tile _defaultDecoratorTile;

    public override IEnumerator Play(Context context)
    {
        var duration = 0.3f;

        var positions = new List<Vector3Int>();

        for (int x = 0; x < WordleTextEditor.NumCharsPerLine; x++)
        {
            for (int y = 0; y < WordleTextEditor.MaxNumLines; y++)
            {
                var position = new Vector3Int(x, y, 0);
                positions.Add(position);
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

            _decoratorTilemapAnimator.SetTileDelayed(position, _defaultDecoratorTile, duration / 2f);
            _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
            _letterTilemapAnimator.SetTileDelayed(position, null, duration / 2f);
            _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
        }
    }
}
