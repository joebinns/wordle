using System.Collections;
using Tilemaps;
using UnityEngine;

public class ShowSolutionAnimation : Animation
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
        var word = _wordChecker.Word;
        for (int i = 0; i < word.Length; i++)
        {
            var characterPosition = new Vector3Int(i, -WordleTextEditor.MaxNumLines, 0);
            var character = word[i];
            var tile = TilemapUtilities.FindTileByCharacter(character);
            _letterTilemapAnimator.SetTile(characterPosition, tile);
            _letterTilemapAnimator.SetRotation(characterPosition, Vector3.right * -90f);
        }
        
        var delay = 0.3f;
        
        var position = new Vector3Int(0, -(_decoratorTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.y - 1), 0);
        for (int x = 0; x < _decoratorTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.x; x++)
        {
            position.x = x;
            ShowTile(position, delay);

            yield return new WaitForSeconds(delay / 2f);
        }
    }
    
    private void ShowTile(Vector3Int position, float duration)
    {
        _letterTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.right * -90f, Vector3.zero, duration);
        _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.right * -90f, Vector3.zero, duration);
    }
}
