using System.Collections;
using UnityEngine;

public abstract class Animation : MonoBehaviour
{
    public abstract IEnumerator Play(Context context);
    
    public struct Context
    {
        public char Character;
        
        public Context(char character)
        {
            this.Character = character;
        }
    }
}
