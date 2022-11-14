using System.Collections;
using System.Collections.Generic;
using Audio;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _blockTilemapAnimator;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;

    [SerializeField] private Color _hoverColor;
    
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
    
    public void RevealGuessTiles(Dictionary<Vector3Int, TileState> positionToTileState)
    {
        StartCoroutine(RevealGuessTilesCoroutine(positionToTileState));
    }

    private IEnumerator RevealGuessTilesCoroutine(Dictionary<Vector3Int, TileState> positionToTileState)
    {
        var duration = 0.6f;
        var totalDuration = duration * positionToTileState.Count;
        yield return new WaitForSeconds(totalDuration);
        foreach (var position in positionToTileState.Keys)
        {
            var tileState = positionToTileState[position];
            //_blockTilemapAnimator.GetComponent<BlockTilemapHandler>().SetTileStateCautious(position, tileState);

            var isTileUpdated = tileState >
                              _blockTilemapAnimator.GetComponent<BlockTilemapHandler>().PositionToTileState[position];
            var tile = _blockTilemapAnimator.GetComponent<BlockTilemapHandler>().TileStateToTile(tileState);
            if (isTileUpdated)
            {
                _blockTilemapAnimator.GetComponent<BlockTilemapHandler>().SetTileStateDelayed(position, tileState, duration / 2f);
                _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, duration);
                _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
            }
        }
    }
    
    public void PressTile(char character)
    {
        var name = character.ToString();
        if (_keyboardLetterTilemapTracker.Contains(name))
        {
            var position = _keyboardLetterTilemapTracker.TileNameToPosition(name);
            _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
            _blockTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
            _blockTilemapAnimator.FlashTileColor(position, Color.white, _hoverColor, 0.05f);
        }
    }

    private void HoverTile(Vector3Int position)
    {
        _blockTilemapAnimator.SetColor(position, _hoverColor);
    }

    private void UnHoverTile(Vector3Int position)
    {
        _blockTilemapAnimator.SetColor(position, Color.white);
    }
}
