using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public abstract class AnimationsController : MonoBehaviour
{
    private Queue<AnimationCall> _animationCalls = new Queue<AnimationCall>();
    protected Queue<AnimationCall> AnimationCalls => _animationCalls;

    protected virtual void Start()
    {
        StartCoroutine(UpdateQueue());
    }

    private IEnumerator UpdateQueue()
    {
        while (true)
        {
            if (_animationCalls.Count > 0)
            {
                var animationCall = _animationCalls.Dequeue();
                yield return animationCall.Animation.Play(animationCall.Context);
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
