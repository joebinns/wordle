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

    [SerializeField] private ParticleSystem _semiParticleSystem;
    [SerializeField] private ParticleSystem _fullParticleSystem;

    private WordleTextEditor _wordleTextEditor;

    private void Awake()
    {
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

    public void SetLetter(Vector3Int position, Tile tile)
    {
        _letterTilemapAnimator.SetTileDelayed(position, tile, 0f);
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
        // Set particle system speed
        var main = particleSystem.main;

        // Set particles start position
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;
        emitParams.applyShapeToPosition = true;

        // Trigger particle emission burst
        particleSystem.Emit(emitParams, 6);
    }

    public void HighlightTiles(List<Vector3Int> positions)
    {
        foreach (var position in positions)
        {
            var duration = 0.5f;
            _blockTilemapAnimator.FlashTile(position, _default, _select, duration * 1.2f);
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
    
    private void ShakeTiles(List<Vector3Int> positions, Vector3 a, Vector3 b, float duration = 0.25f)
    {
        foreach (var position in positions)
        {
            _blockTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothShakeTileOnce(position, a, b, duration);
        }
    }

    public void ToggleSolutionTilesVisibility(bool shouldShow, float delay = 0.3f)
    {
        StartCoroutine(ToggleSolutionTilesVisibilityCoroutine(shouldShow, delay));
    }

    private IEnumerator ToggleSolutionTilesVisibilityCoroutine(bool shouldShow, float delay)
    {
        var duration = delay * WordleTextEditor.NumCharsPerLine;
        //if (delay != 0f) { yield return new WaitForSeconds(duration); }

        duration /= 2f;
        var position = new Vector3Int(0, -(_blockTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.y - 1), 0);
        for (int x = 0; x < _blockTilemapAnimator.GetComponent<DecoratorTilemapHandler>().Tilemap.size.x; x++)
        {
            position.x = x;

            var a = shouldShow ? Vector3.right * 90f : Vector3.zero;
            var b = shouldShow ? Vector3.zero : Vector3.right * 90f;
            _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, a, b, duration);
            _letterTilemapAnimator.SmoothHalfFlipTileOnce(position, a, b, duration);

            if (delay != 0f) { yield return new WaitForSeconds(delay); }
        }
    }

    public void ToggleTileVisibilities(List<Vector3Int> positions, float period)
    {
        StartCoroutine(ToggleTileVisibilitiesCoroutine(positions, period));
    }
    
    private IEnumerator ToggleTileVisibilitiesCoroutine(List<Vector3Int> positions, float period)
    {
        var hiddenEuler = Vector3.right * 90f;
        var shownEuler = Vector3.zero;
        foreach (var position in positions)
        {
            var isHidden = true;
            _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, isHidden ? hiddenEuler : shownEuler, isHidden ? shownEuler : hiddenEuler, period);
            yield return new WaitForSeconds(period);
        }
    }

    public void RevealGuessTiles(Dictionary<Vector3Int, Tile> positionToTile)
    {
        StartCoroutine(RevealGuessTilesCoroutine(positionToTile));
    }

    private IEnumerator RevealGuessTilesCoroutine(Dictionary<Vector3Int, Tile> positionToTile)
    {
        var duration = 0.6f;
        var pitch = 1.0f;
        foreach (var position in positionToTile.Keys)
        {
            var tile = positionToTile[position];

            // TODO: Identify which tile needs which sound and particle effects
            // TODO: Increase pitch
            AudioManager.Instance.SetPitch("Flip", pitch);
            AudioManager.Instance.Play("Flip");

            _blockTilemapAnimator.SetTileDelayed(position, tile, duration / 2f);
            _blockTilemapAnimator.SmoothHalfFlipTileOnce(position, Vector3.zero, Vector3.right * 180f, duration);
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
}
