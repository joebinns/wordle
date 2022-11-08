using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextTilemap _textManager;

    private void Awake()
    {
        _textManager = FindObjectOfType<TextTilemap>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if ((c == '\n') || (c == '\r')) // enter/return
                {
                    _textManager.EnterText();
                }
                else
                {
                    _textManager.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
