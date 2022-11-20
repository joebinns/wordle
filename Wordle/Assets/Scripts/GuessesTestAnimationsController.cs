using UnityEngine;

public class GuessesTestAnimationsController : AnimationsController
{
    [SerializeField] private Animation InitialiseAllAnimation;
    [SerializeField] private Animation ShowGuessesAnimation;
    [SerializeField] private Animation SetTextAnimation; 
    [SerializeField] private Animation ShowAccuracyAnimation;
    [SerializeField] private Animation ShowSolutionAnimation;
    [SerializeField] private Animation ClearAllAnimation;
    
    protected override void Start()
    {
        base.Start();
        AnimationCalls.Enqueue(new AnimationCall(InitialiseAllAnimation, new Animation.Context()));
        AnimationCalls.Enqueue(new AnimationCall(ShowGuessesAnimation, new Animation.Context()));
    }
}
