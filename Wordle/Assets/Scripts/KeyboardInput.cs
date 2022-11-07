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
                if ((c == '\n') || (c == '\r')) // enter/return
                {
                    _textManager.CheckIfTextIsValid();
                }
                else
                {
                    _textManager.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
