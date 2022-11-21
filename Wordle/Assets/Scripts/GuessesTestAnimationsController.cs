using UnityEngine;

public class GuessesTestAnimationsController : AnimationsController
{
    [SerializeField] private Animation InitialiseAllAnimation;
    [SerializeField] private Animation ShowGuessesAnimation;
    [SerializeField] private Animation SetTextAnimation; 
    [SerializeField] private Animation ShakeAnimation;
    [SerializeField] private Animation SubmitGuessAnimation;
    [SerializeField] private Animation ShowSolutionAnimation;
    [SerializeField] private Animation ClearAllAnimation;

    private void OnEnable()
    {
        WordleTextEditor.OnTextChanged += EnqueueSetTextAnimation;
        WordleTextEditor.OnTextChanged += EnqueueSubmitGuessAnimation;
        WordleTextEditor.OnInvalidInput += EnqueueShakeAnimation;
        WordleTextEditor.OnWordleTextEditorDisabled += EnqueueShowSolutionAnimation;
    }

    private void OnDisable()
    {
        WordleTextEditor.OnTextChanged -= EnqueueSetTextAnimation;
        WordleTextEditor.OnTextChanged -= EnqueueSubmitGuessAnimation;
        WordleTextEditor.OnInvalidInput -= EnqueueShakeAnimation;
        WordleTextEditor.OnWordleTextEditorDisabled -= EnqueueShowSolutionAnimation;
    }

    protected override void Start()
    {
        base.Start();
        AnimationCalls.Enqueue(new AnimationCall(InitialiseAllAnimation, new Animation.Context()));
        AnimationCalls.Enqueue(new AnimationCall(ShowGuessesAnimation, new Animation.Context()));
    }

    private void EnqueueSubmitGuessAnimation(char character)
    {
        //PlayAnimation(new AnimationCall(SubmitGuessAnimation, new Animation.Context(character)));
        AnimationCalls.Enqueue(new AnimationCall(SubmitGuessAnimation, new Animation.Context(character)));
    }

    private void EnqueueSetTextAnimation(char character)
    {
        PlayAnimation(new AnimationCall(SetTextAnimation, new Animation.Context(character)));
    }

    private void EnqueueShakeAnimation()
    {
        PlayAnimation(new AnimationCall(ShakeAnimation, new Animation.Context()));
    }
    
    private void EnqueueShowSolutionAnimation() // TODO: Call this when text editor is disabled
    {
        //PlayAnimation(new AnimationCall(ShowSolutionAnimation, new Animation.Context()));
        AnimationCalls.Enqueue(new AnimationCall(ShowSolutionAnimation, new Animation.Context()));
    }
}
