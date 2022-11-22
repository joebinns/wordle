using System.Collections;
using System.Collections.Generic;
using Tilemaps;
using UnityEngine;

public class ClickAnimation : Animation
{
    [SerializeField] private TextTilemapTracker _textTilemapTracker;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private HoverableTilemap _hoverableTilemap;
    
    public override IEnumerator Play(Context context)
    {
        var character = context.Character;
        var position = _textTilemapTracker.CharacterToPosition(character);
        var duration = 0.1f;
        ClickTile(position, duration);
        yield return new WaitForSeconds(duration);
    }
    
    private void ClickTile(Vector3Int position, float duration)
    {
        _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, duration);
        _decoratorTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, duration);
        _decoratorTilemapAnimator.FlashTileColor(position, Color.white, _hoverableTilemap.HoverColor, duration);
    }
}
