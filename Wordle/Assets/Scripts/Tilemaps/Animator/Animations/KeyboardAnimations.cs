using System.Collections;
using System.Collections.Generic;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private HoverableTilemap _hoverableTilemap;

    public void ClickTile(Vector3Int position)
    {
        _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
        _decoratorTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
        _decoratorTilemapAnimator.FlashTileColor(position, Color.white, _hoverableTilemap.HoverColor, 0.1f);
    }
    
    public void RevealGuessTiles(Dictionary<Vector3Int, Tile> positionToTile)
    {
        StartCoroutine(RevealGuessTilesCoroutine(positionToTile));
    }

    // TODO: Convert this to take a dictionary of positions to tiles
    private IEnumerator RevealGuessTilesCoroutine(Dictionary<Vector3Int, Tile> positionToTile)
    {
        var duration = 0.6f;
        var totalDuration = duration * positionToTile.Count;
        yield return new WaitForSeconds(totalDuration);
        foreach (var position in positionToTile.Keys)
        {
            var tile = positionToTile[position];
            _decoratorTilemapAnimator.GetComponent<DecoratorTilemapHandler>().SetTileDelayed(position, tile, duration / 2f);
            _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
            _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
        }
    }
}
