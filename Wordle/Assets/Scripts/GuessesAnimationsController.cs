using System;
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
        // TODO: Get filled tiles from WordleTextEditor.Text's latest row.
        _wordleTextEditor.GetFinalLine();

        // TODO: Get empty tiles from --||--.
    }
}
