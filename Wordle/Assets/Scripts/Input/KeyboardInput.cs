using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextEditor _textEditor;

    public static event Action<char> OnKeyDown;
    
    private void Awake()
    {
        _textEditor = FindObjectOfType<TextEditor>();
    }

    private void Update()
    {
        UpdateKeyDown();
    }

    private void UpdateKeyDown()
    {
        if (Input.anyKeyDown)
        {
            foreach (char character in Input.inputString)
            {
                OnKeyDown?.Invoke(character);
            }
        }
    }
}
