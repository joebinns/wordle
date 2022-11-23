using System.Collections;
using Tilemaps;
using UnityEngine;

public class HideSolutionAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;

    private WordChecker _wordChecker;

    private void Awake()
    {
        _wordChecker = FindObjectOfType<WordChecker>();
    }
    
    public override IEnumerator Play(Context context)
    {
        var delay = 0.3f;
        
        var position = new Vector3Int(0, -(_decoratorTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.y - 1), 0);
        for (int x = 0; x < _decoratorTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.x; x++)
        {
            position.x = x;
            HideTile(position, delay);

            yield return new WaitForSeconds(delay / 2f);
        }
    }
    
    private void HideTile(Vector3Int position, float duration)
    {
        _letterTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * -90f, duration);
        _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * -90f, duration);
    }
}
