using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuessesAnimationsController : MonoBehaviour
{
    private GuessesAnimations _guessesAnimations;
    private WordleTextEditor _wordleTextEditor;

    private void Awake()
    {
        _guessesAnimations = GetComponent<GuessesAnimations>();
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

    private void OnEnable()
    {
        WordleTextEditor.OnInvalidInput += Shake;
    }
    
    private void OnDisable()
    {
        WordleTextEditor.OnInvalidInput -= Shake;
    }

    private void Shake()
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        Debug.Log("line index: " +  lineIndex);
        
        var fullIndices = _wordleTextEditor.GetFullIndices(lineIndex);
        var fullPositions = IndicesToPositions(lineIndex, fullIndices);
        
        var empty = _wordleTextEditor.GetEmptyIndices(lineIndex);
        var emptyPositions = IndicesToPositions(lineIndex, empty);

        _guessesAnimations.HighlightTiles(emptyPositions);
        _guessesAnimations.ShakeTilesReactive(emptyPositions, fullPositions, Vector3.zero, Vector3.right * 1f);
    }

    private List<Vector3Int> IndicesToPositions(int lineIndex, List<int> indices)
    {
        return indices.Select(characterIndex => new Vector3Int(characterIndex, -lineIndex, 0)).ToList();
    }
}
