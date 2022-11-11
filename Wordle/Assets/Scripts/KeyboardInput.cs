using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextEditor _textEditor;
    private MouseInput _mouseInput;

    private void Awake()
    {
        _textEditor = FindObjectOfType<TextEditor>();
        _mouseInput = FindObjectOfType<MouseInput>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if ((c == '\n') || (c == '\r')) // enter/return
                {
                    _textEditor.EnterText();
                }
                
                else
                {
                    _mouseInput.PressTile(c);
                    _textEditor.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
