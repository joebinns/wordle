using System.Collections;
using System.Collections.Generic;
using Tilemaps;
using UnityEngine;

public class KeyboardAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _blockTilemapAnimator;
    [SerializeField] private LetterTilemapTracker _keyboardLetterTilemapTracker;


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
                _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
                _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
            }
        }
    }
    
    public void PressTile(char character)
    {
        if (_keyboardLetterTilemapTracker.Contains(character))
        {
            var position = _keyboardLetterTilemapTracker.CharacterToPosition(character);
            _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
            _blockTilemapAnimator.SmoothLoopTilePositionOnce(position, Vector3.zero, Vector3.down * 0.15f, 0.1f);
            //_blockTilemapAnimator.FlashTileColor(position, Color.white, _hoverColor, 0.05f); // TODO: Need to get hover color
        }
    }
}
