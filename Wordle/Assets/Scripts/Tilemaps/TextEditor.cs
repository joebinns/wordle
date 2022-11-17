using System;
using System.Text;
using UnityEngine;

public abstract class TextEditor : MonoBehaviour
{
    protected string Text = "";

    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }

    public static event Action OnInvalidInput; // TODO: Subscribe to this somewhere to run animations.
    public static event Action<char> OnTextChanged; // TODO: Subscribe to this somewhere to change tiles.
    
    public void AmendText(char character)
    {
        var isInputValid = IsInputValid(character);
        if (!isInputValid) { OnInvalidInput?.Invoke(); return; }

        if (character == '\b') { Text = Text.Remove(Text.Length-1); }
        else { Text += character; }
        OnTextChanged?.Invoke(character);
        
        Debug.Log(Text);
    }

    protected abstract bool IsInputValid(char character);

    private void Reset()
    {
        Text = "";
    }
}
