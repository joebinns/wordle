using UnityEngine;

public class GuessesTestAnimationsController : AnimationsController
{
    [SerializeField] private Animation InitialiseAllAnimation;
    [SerializeField] private Animation ShowGuessesAnimation;
    [SerializeField] private Animation ShowAccuracyAnimation;
    [SerializeField] private Animation ShowSolutionAnimation;
    [SerializeField] private Animation ClearAllAnimation;
    
    protected override void Start()
    {
        base.Start();
        AnimationQueue.Enqueue(InitialiseAllAnimation);
        AnimationQueue.Enqueue(ShowGuessesAnimation);
    }
}
