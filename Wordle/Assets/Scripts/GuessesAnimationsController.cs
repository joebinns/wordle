using UnityEngine;

public class GuessesAnimationsController : AnimationsController
{
    [SerializeField] private Animation _initialiseAllAnimation;
    [SerializeField] private Animation _showGuessesAnimation;
    [SerializeField] private Animation _setTextAnimation; 
    [SerializeField] private Animation _shakeAnimation;
    [SerializeField] private Animation _submitGuessAnimation;
    [SerializeField] private Animation _showSolutionAnimation;
    [SerializeField] private Animation _resetAnimation;
    [SerializeField] private Animation _hideSolutionAnimation;

    private bool _isSolutionVisible = false;

    private void OnEnable()
    {
        WordleTextEditor.OnTextChanged += EnqueueSetTextAnimation;
        WordleTextEditor.OnTextChanged += EnqueueSubmitGuessAnimation;
        WordleTextEditor.OnInvalidInput += EnqueueShakeAnimation;
        WordleTextEditor.OnFail += EnqueueShowSolutionAnimation;
        GameManager.Instance.OnGameReset += EnqueueResetAnimation;
    }

    private void OnDisable()
    {
        WordleTextEditor.OnTextChanged -= EnqueueSetTextAnimation;
        WordleTextEditor.OnTextChanged -= EnqueueSubmitGuessAnimation;
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

    private void EnqueueSubmitGuessAnimation(char character)
    {
        AnimationCalls.Enqueue(new AnimationCall(_submitGuessAnimation, new Animation.Context(character)));
    }

    private void EnqueueSetTextAnimation(char character)
    {
        PlayAnimation(new AnimationCall(_setTextAnimation, new Animation.Context(character)));
    }

    private void EnqueueShakeAnimation()
    {
        PlayAnimation(new AnimationCall(_shakeAnimation, new Animation.Context()));
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
        _isSolutionVisible = false;
    }
}
