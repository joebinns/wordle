using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationsController : MonoBehaviour
{
    private Queue<Animation> _animationQueue = new Queue<Animation>();
    protected Queue<Animation> AnimationQueue => _animationQueue;

    protected virtual void Start()
    {
        StartCoroutine(UpdateQueue());
    }

    private IEnumerator UpdateQueue()
    {
        while (true)
        {
            if (_animationQueue.Count > 0)
            {
                yield return _animationQueue.Dequeue().Play();
            }
            yield return null;
        }
    }
}
