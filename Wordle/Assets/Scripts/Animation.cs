using System.Collections;
using UnityEngine;

public abstract class Animation : MonoBehaviour
{
    public abstract IEnumerator Play();
}
