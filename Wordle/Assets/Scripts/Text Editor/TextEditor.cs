using System;
using System.Text;
using UnityEngine;

public abstract class TextEditor : MonoBehaviour
{
    public bool IsEnabled = true;
    public const int MaxNumLines = 6;
    
    public string Text => _text;
    protected string _text = "";

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
        if (character == '\b') { _text = _text.Remove(_text.Length-1); }
        else { _text += character; }
        OnTextChanged?.Invoke(character);
    }

    public int GetFinalLineIndex()
    {
        return(GetLines().Length - 1);
    }
    
    public string GetFinalLine()
    {
        return(GetLines()[^1]);
    }
    
    public string GetLine(int index)
    {
        return(GetLines()[index]);
    }

    public bool HasLine(int index)
    {
        return index < GetLines().Length;
    }
    
    protected string[] GetLines()
    {
        var lines = Text.Split(new[] { '\r' }, StringSplitOptions.None);
        return lines;
    }

    protected abstract bool IsInputValid(char character);

    protected virtual void Reset()
    {
        _text = "";
        IsEnabled = true;
    }
}
