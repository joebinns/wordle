using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShakeAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private Tile _default;
    [SerializeField] private Tile _select;
        
    private WordleTextEditor _wordleTextEditor;

    private void Awake()
    {
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

    public override IEnumerator Play(Context context)
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        
        var fullIndices = _wordleTextEditor.GetFullIndices(lineIndex);
        var fullPositions = IndicesToPositions(fullIndices);
        
        var emptyIndices = _wordleTextEditor.GetEmptyIndices(lineIndex);
        var emptyPositions = IndicesToPositions(emptyIndices);

        var areAllPositionsFull = emptyPositions.Count == 0;

        HighlightTiles(areAllPositionsFull ? fullPositions : emptyPositions);
        yield return StartCoroutine(ShakeTilesReactive(areAllPositionsFull ? fullPositions : emptyPositions, areAllPositionsFull ? emptyPositions : fullPositions, Vector3.zero, areAllPositionsFull ? Vector3.left : Vector3.right));
    }
    
    private void HighlightTiles(List<Vector3Int> positions)
    {
        foreach (var position in positions)
        {
            var duration = 0.5f;
            _decoratorTilemapAnimator.FlashTile(position, _default, _select, duration * 1.2f);
        }
    }
    
    private IEnumerator ShakeTilesReactive(List<Vector3Int> primary, List<Vector3Int> secondary, Vector3 a, Vector3 b)
    {
        var duration = 0.25f;
        ShakeTiles(primary, a, b, duration);
        yield return new WaitForSeconds(duration / 2f);
        LoopTilePositions(secondary, a, -b, duration / 2f);
    }
    
    private void ShakeTiles(List<Vector3Int> positions, Vector3 a, Vector3 b, float duration = 0.25f)
    {
        foreach (var position in positions)
        {
            _decoratorTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
        }
    }
    
    private void LoopTilePositions(List<Vector3Int> positions, Vector3 a, Vector3 b, float duration = 0.25f)
    {
        foreach (var position in positions)
        {
            _decoratorTilemapAnimator.SmoothLoopTilePositionOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothLoopTilePositionOnce(position, a, b, duration);
        }
    }
    
    private List<Vector3Int> IndicesToPositions(List<int> indices)
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        return indices.Select(characterIndex => new Vector3Int(characterIndex, -lineIndex, 0)).ToList();
    }
}
