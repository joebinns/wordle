using System;
using UnityEngine;

public abstract class TextEditor : MonoBehaviour
{
    protected string Text;

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }

    public static event Action OnInvalidInput; // TODO: Subscribe to this somewhere to run animations
    
    public void AmendText(char character)
    {
        var isInputValid = IsInputValid(character);
        if (!isInputValid) { OnInvalidInput?.Invoke(); return; }
        Text += character;
    }

    protected abstract bool IsInputValid(char character);

    private void Reset()
    {
        Text = "";
    }
}
