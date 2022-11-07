using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                Debug.Log(c);
                if (c == '\b') // has backspace/delete been pressed?
                {
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                }
                else
                {
                }
            }
        }
    }
}
