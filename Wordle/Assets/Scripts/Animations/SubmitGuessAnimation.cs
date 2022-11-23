using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SubmitGuessAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    [SerializeField] private LetterTilemapHandler _letterTilemapHandler;
    [SerializeField] private DecoratorTilemapHandler _decoratorTilemapHandler;
    [SerializeField] private ParticleSystem _fullParticleSystem;
    [SerializeField] private ParticleSystem _semiParticleSystem;
    
    private WordleTextEditor _wordleTextEditor;
    private WordChecker _wordChecker;

    private void Awake()
    {
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    public override IEnumerator Play(Context context)
    {
        var character = context.Character;
        if (character == '\r')
        {
            var word = _wordleTextEditor.GetLine(context.LineIndex - 1);
            var indexToTileState = _wordChecker.GetTileStates(word);
            var indices = indexToTileState.Keys.ToList();
            var positionToTile = new Dictionary<Vector3Int, Tile>();
            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                var position = new Vector3Int(index, -context.LineIndex, 0);
                position.y++;
                var tile = _decoratorTilemapHandler.TileStateToTile(indexToTileState[index]);
                positionToTile[position] = tile;
            }
            yield return StartCoroutine(RevealGuessTiles(positionToTile));
        }
        
        yield return null;
    }

    private IEnumerator RevealGuessTiles(Dictionary<Vector3Int, Tile> positionToTile)
    {
        var duration = 0.6f;
        var pitch = 1.0f;
        foreach (var position in positionToTile.Keys)
        {
            var tile = positionToTile[position];
            
            //AudioManager.Instance.SetPitch("Flip", pitch);
            //AudioManager.Instance.Play("Flip");

            _decoratorTilemapAnimator.SetTileDelayed(position, tile, duration / 2f);
            _decoratorTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
            _letterTilemapAnimator.SmoothTrickHalfFlipTileOnce(position, duration);
            
            var worldPosition = _letterTilemapHandler.Tilemap.GetCellCenterWorld(position);
            if (tile.name == "correct_guess")
            {
                PlayParticleSystem(_fullParticleSystem, worldPosition);
            }
            else
            {
                StopParticleSystem(_fullParticleSystem);
            }
            yield return new WaitForSeconds(duration / 2f);
            if (tile.name == "semi_correct_guess")
            {
                PlayParticleSystemBurst(_semiParticleSystem, worldPosition);
            }
            yield return new WaitForSeconds(duration / 2f);
            
            pitch += 0.5f;
        }
        StopParticleSystem(_fullParticleSystem);
    }
    
    private void PlayParticleSystem(ParticleSystem particleSystem, Vector3 position)
    {
        particleSystem.transform.position = position;
        particleSystem.Play();
    }
    
    private void StopParticleSystem(ParticleSystem particleSystem)
    {
        particleSystem.Stop();
    }
    
    private void PlayParticleSystemBurst(ParticleSystem particleSystem, Vector3 position)
    {
        // Set particles start position
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;
        emitParams.applyShapeToPosition = true;

        // Trigger particle emission burst
        particleSystem.Emit(emitParams, 6);
    }
}
