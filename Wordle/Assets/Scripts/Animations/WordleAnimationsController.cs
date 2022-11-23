using System;
using UnityEngine;

public class WordleAnimationsController : AnimationsController
{
    private InitialiseAllAnimation _initialiseAllAnimation;
    private ShowGuessesAnimation _showGuessesAnimation;
    private SetTextAnimation _setTextAnimation; 
    private ShakeAnimation _shakeAnimation;
    private SubmitGuessAnimation _submitGuessAnimation;
    private ShowSolutionAnimation _showSolutionAnimation;
    private ResetAnimation _resetAnimation;
    private HideSolutionAnimation _hideSolutionAnimation;
    
    private ClickAnimation _clickAnimation;
    private RevealAccuracyAnimation _revealAccuracyAnimation;
    private KeyboardResetAnimation _keyboardResetAnimation;

    private WordleTextEditor _wordleTextEditor;
    
    private bool _isSolutionVisible = false;

    private void Awake()
    {
        _initialiseAllAnimation = GetComponentInChildren<InitialiseAllAnimation>();
        _showGuessesAnimation = GetComponentInChildren<ShowGuessesAnimation>();
        _setTextAnimation = GetComponentInChildren<SetTextAnimation>();
        _shakeAnimation = GetComponentInChildren<ShakeAnimation>();
        _submitGuessAnimation = GetComponentInChildren<SubmitGuessAnimation>();
        _showSolutionAnimation = GetComponentInChildren<ShowSolutionAnimation>();
        _resetAnimation = GetComponentInChildren<ResetAnimation>();
        _hideSolutionAnimation = GetComponentInChildren<HideSolutionAnimation>();

        _clickAnimation = GetComponentInChildren<ClickAnimation>();
        _revealAccuracyAnimation = GetComponentInChildren<RevealAccuracyAnimation>();
        _keyboardResetAnimation = GetComponentInChildren<KeyboardResetAnimation>();
        
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

    private void OnEnable()
    {
        WordleTextEditor.OnTextChanged += TextInputAnimation;
        WordleTextEditor.OnInvalidInput += EnqueueShakeAnimation;
        WordleTextEditor.OnFail += EnqueueShowSolutionAnimation;
        GameManager.Instance.OnGameReset += EnqueueResetAnimation;
    }

    private void OnDisable()
    {
        WordleTextEditor.OnTextChanged -= TextInputAnimation;
        WordleTextEditor.OnInvalidInput -= EnqueueShakeAnimation;
        WordleTextEditor.OnFail -= EnqueueShowSolutionAnimation;
        GameManager.Instance.OnGameReset -= EnqueueResetAnimation;
    }

    protected override void Start()
    {
        base.Start();
        AnimationCalls.Enqueue(new AnimationCall(_initialiseAllAnimation, new Animation.Context()));
        AnimationCalls.Enqueue(new AnimationCall(_showGuessesAnimation, new Animation.Context()));
    }

    private void TextInputAnimation(char character)
    {
        PlayAnimation(new AnimationCall(_setTextAnimation, new Animation.Context(character, _wordleTextEditor.GetFinalLineIndex())));
        AnimationCalls.Enqueue(new AnimationCall(_submitGuessAnimation, new Animation.Context(character, _wordleTextEditor.GetFinalLineIndex())));
        AnimationCalls.Enqueue(new AnimationCall(_revealAccuracyAnimation, new Animation.Context(character, _wordleTextEditor.GetFinalLineIndex())));
    }

    private void EnqueueShakeAnimation()
    {
        AnimationCalls.Enqueue(new AnimationCall(_shakeAnimation, new Animation.Context()));
    }
    
    private void EnqueueShowSolutionAnimation()
    {
        AnimationCalls.Enqueue(new AnimationCall(_showSolutionAnimation, new Animation.Context()));
        _isSolutionVisible = true;
    }
    
    private void EnqueueResetAnimation()
    {
        if (_isSolutionVisible)
        {
            AnimationCalls.Enqueue(new AnimationCall(_hideSolutionAnimation, new Animation.Context()));
        }
        AnimationCalls.Enqueue(new AnimationCall(_resetAnimation, new Animation.Context()));
        AnimationCalls.Enqueue(new AnimationCall(_keyboardResetAnimation, new Animation.Context()));
        _isSolutionVisible = false;
    }
    
    public void PlayClickAnimation(char character)
    {
        PlayAnimation(new AnimationCall(_clickAnimation, new Animation.Context(character, _wordleTextEditor.GetFinalLineIndex())));
    }
}
