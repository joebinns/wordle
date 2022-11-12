using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextEditor _textEditor;
    private KeyboardAnimations _keyboardAnimations;

    private void Awake()
    {
        _textEditor = FindObjectOfType<TextEditor>();
        _keyboardAnimations = FindObjectOfType<KeyboardAnimations>();
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
                    _keyboardAnimations.PressTile(c);
                    _textEditor.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
