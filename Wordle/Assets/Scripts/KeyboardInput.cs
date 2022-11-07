using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextManager _textManager;

    private void Awake()
    {
        _textManager = FindObjectOfType<TextManager>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                // TODO: Pass character to textmanager
                
                /*
                if (c == '\b') // has backspace/delete been pressed?
                {
                    _textManager.DeleteCharacterAtPreviousIndex();
                }
                */
                if ((c == '\n') || (c == '\r')) // enter/return
                {
                    // IF THERE IS AN ACCEPTABLE WORD, THEN SUBMIT
                }
                else
                {
                    _textManager.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
