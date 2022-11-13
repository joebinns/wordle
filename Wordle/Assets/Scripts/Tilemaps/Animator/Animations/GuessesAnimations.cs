using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GuessesAnimations : MonoBehaviour
{
    [SerializeField] private TilemapAnimator _blockTilemapAnimator;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private LetterTilemapHandler _letterTilemapHandler;

    [SerializeField] private Tile _default;
    [SerializeField] private Tile _select;
    
    private TextEditor _textEditor;
    
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

    private void Awake()
    {
        _textEditor = FindObjectOfType<TextEditor>();
    }

    public void HighlightTiles(List<Vector3Int> positions)
    {
        foreach (var position in positions)
        {
            var duration = 0.5f;
            _blockTilemapAnimator.FlashTile(position, _default, _select, duration * 1.2f);
            //_blockTilemapAnimator.SmoothFlipTileOnce(position, duration);
            //_letterTilemapAnimator.SmoothFlipTileOnce(position, duration, true);
        }
    }
    
    public void ShakeTilesReactive(List<Vector3Int> primary, List<Vector3Int> secondary, Vector3 a, Vector3 b)
    {
        StartCoroutine(ShakeTilesReactiveCoroutine(primary, secondary, a, b));
    }

    private IEnumerator ShakeTilesReactiveCoroutine(List<Vector3Int> primary, List<Vector3Int> secondary, Vector3 a, Vector3 b)
    {
        var duration = 0.25f;
        ShakeTiles(primary, a, b, duration);
        yield return new WaitForSeconds(duration / 2f);
        LoopTilePositions(secondary, a, -b, duration / 2f);
    }

    private void LoopTilePositions(List<Vector3Int> positions, Vector3 a, Vector3 b, float duration = 0.25f)
    {
        foreach (var position in positions)
        {
            _blockTilemapAnimator.SmoothLoopTilePositionOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, a, b, duration);
        }
    }
    
    public void ShakeTiles(List<Vector3Int> positions, Vector3 a, Vector3 b, float duration = 0.25f)
    {
        foreach (var position in positions)
        {
            _blockTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
        }
    }
    
    public void RevealGuessTiles(Dictionary<Vector3Int, Tile> positionToTile)
    {
        StartCoroutine(RevealGuessTilesCoroutine(positionToTile));
    }

    private IEnumerator RevealGuessTilesCoroutine(Dictionary<Vector3Int, Tile> positionToTile)
    {
        var duration = 0.3f;
        var pitch = 1.0f;
        foreach (var position in positionToTile.Keys)
        {
            var tile = positionToTile[position];

            // TODO: Identify which tile needs which sound and particle effects
            // TODO: Increase pitch
            AudioManager.Instance.SetPitch("Flip", pitch);
            AudioManager.Instance.Play("Flip");

            _blockTilemapAnimator.SetTileDelayed(position, tile, duration / 2f);
            _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, duration);
            _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
            //_blockTilemapAnimator.OscillateHalfFlipTileOnce(position, duration / 2f);
            //_letterTilemapAnimator.OscillateHalfFlipTileOnce(position, duration / 2f);
            yield return new WaitForSeconds(duration);
            pitch += 0.5f;
        }
    }
    
    public void PressTile(char character)
    {
        /*
        var name = character.ToString();
        if (_keyboardLetterTilemapTracker.Contains(name))
        {
            var position = _keyboardLetterTilemapTracker.TileNameToPosition(name);
            _tilemapAnimator.SmoothLoopTilePosition(position);
        }
        */
    }

    private void HoverTile(Vector3Int position)
    {
        var color = Color.white;
        color.a = 0.8f;
        _blockTilemapAnimator.SetColor(position, color);
    }

    private void UnHoverTile(Vector3Int position)
    {
        var color = Color.white;
        _blockTilemapAnimator.SetColor(position, color);
    }
}
