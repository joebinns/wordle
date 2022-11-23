using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationsController : MonoBehaviour
{
    private Queue<AnimationCall> _animationCalls = new Queue<AnimationCall>();
    protected Queue<AnimationCall> AnimationCalls => _animationCalls;

    protected virtual void Start()
    {
        StartCoroutine(UpdateQueue());
    }

    protected void PlayAnimation(AnimationCall animationCall)
    {
        StartCoroutine(PlayAnimationCoroutine(animationCall));
    }
    
    private IEnumerator PlayAnimationCoroutine(AnimationCall animationCall)
    {
        yield return animationCall.Animation.Play(animationCall.Context);
    }

    private IEnumerator UpdateQueue()
    {
        while (true)
        {
            if (_animationCalls.Count > 0)
            {
                var animationCall = _animationCalls.Dequeue();
                yield return PlayAnimationCoroutine(animationCall);
            }
            yield return null;
        }
    }

    public struct AnimationCall
    {
        public Animation Animation { get; }
        public Animation.Context Context { get; }
    
        public AnimationCall(Animation animation, Animation.Context context)
        {
            this.Animation = animation;
            this.Context = context;
        }
    }
}
