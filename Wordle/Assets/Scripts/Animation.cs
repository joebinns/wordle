using System.Collections;
using UnityEngine;

public abstract class Animation : MonoBehaviour
{
    public abstract IEnumerator Play(Context context);
    
    public struct Context
    {
        public char Character;
        public int LineIndex;
        
        public Context(char character, int lineIndex)
        {
            this.Character = character;
            this.LineIndex = lineIndex;
        }
    }
}
