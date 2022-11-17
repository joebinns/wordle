using System;
using System.Collections;
using UnityEngine;

public class WordleTextEditor : TextEditor
{
    private WordChecker _wordChecker;
    public const int NumCharsPerLine = 5;

    private void Awake()
    {
        _wordChecker = FindObjectOfType<WordChecker>();
    }
    
    protected override bool IsInputValid(char character)
    {
        var lines = Text.Split(new[] { '\r' }, StringSplitOptions.None);
        var finalLine = lines[^1];
        foreach (var line in lines)
        {
            Debug.Log("num lines: " + lines.Length + ". line: " + line);
        }
        if (character == '\r')
        {
            var isLineComplete = IsLineComplete(lines, finalLine);
            if (!isLineComplete) { return false; }
        }
        else if (character == '\b')
        {
            var isLineEmpty = finalLine.Length == 0;
            if (isLineEmpty) { return false; }
        }
        else if (character >= 'a' && character <= 'z')
        {
            var isLineFull = finalLine.Length == NumCharsPerLine;
            if (isLineFull) { return false; }
        }
        else
        {
            var isCharacterRecognised = false;
            if (!isCharacterRecognised) { return false; }
        }
        return true;
    }

    private bool IsLineComplete(string[] lines ,string line)
    {
        var isLineFull = line.Length == NumCharsPerLine;
        if (isLineFull)
        {
            var isWordRecognised = _wordChecker.IsWordRecognised(line);
            if (isWordRecognised)
            {
                var isWordUnique = !((IList)lines[..^1]).Contains(line);
                if (isWordUnique)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
