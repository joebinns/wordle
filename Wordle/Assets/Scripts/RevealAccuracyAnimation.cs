using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealAccuracyAnimation : Animation
{
    [SerializeField] private TextTilemapTracker _textTilemapTracker;
    [SerializeField] private DecoratorTilemapHandler _decoratorTilemapHandler;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    
    private WordleTextEditor _wordleTextEditor;
    private WordChecker _wordChecker;

    private void Awake()
    {
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    public override IEnumerator Play(Context context)
    {
        var input = context.Character;
        if (input == '\r')
        {
            var word = _wordleTextEditor.GetLine(_wordleTextEditor.GetFinalLineIndex() - 1);
            var indexToTileState = _wordChecker.GetTileStates(word);
            var indices = indexToTileState.Keys.ToList();
            var positionToTile = new Dictionary<Vector3Int, Tile>();
            for (int i = 0; i < indices.Count; i++)
            {
                var character = word[i];
                var index = indices[i];
                var tileState = indexToTileState[index];
                var characterPosition = _textTilemapTracker.CharacterToPosition(character);
                if (tileState > _decoratorTilemapHandler.PositionToTileState[characterPosition])
                {
                    var tile = _decoratorTilemapHandler.TileStateToTile(tileState);
                    positionToTile[characterPosition] = tile;
                }
            }
            yield return StartCoroutine(RevealGuessTiles(positionToTile));
        }
    }
    
    private IEnumerator RevealGuessTiles(Dictionary<Vector3Int, Tile> positionToTile)
    {
        var duration = 0.6f;
        var totalDuration = duration * WordleTextEditor.NumCharsPerLine;
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
