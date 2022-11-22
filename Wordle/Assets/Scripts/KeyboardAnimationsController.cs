using UnityEngine;

public class KeyboardAnimationsController : AnimationsController
{
    [SerializeField] private Animation _clickAnimation;
    [SerializeField] private Animation _revealAccuracyAnimation;
    [SerializeField] private Animation _keyboardResetAnimation;
    
    private void OnEnable()
    {
        WordleTextEditor.OnTextChanged += PlayRevealAccuracyAnimation;
        GameManager.Instance.OnGameReset += EnqueueResetAnimation;
    }
    
    private void OnDisable()
    {
        WordleTextEditor.OnTextChanged -= PlayRevealAccuracyAnimation;
        GameManager.Instance.OnGameReset -= EnqueueResetAnimation;
    }
    
    public void PlayClickAnimation(char character)
    {
        PlayAnimation(new AnimationCall(_clickAnimation, new Animation.Context(character)));
    }

    private void PlayRevealAccuracyAnimation(char character)
    {
        PlayAnimation(new AnimationCall(_revealAccuracyAnimation, new Animation.Context(character)));
    }
    
    private void EnqueueResetAnimation()
    {
        AnimationCalls.Enqueue(new AnimationCall(_keyboardResetAnimation, new Animation.Context()));
    }
}
