using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordleTextEditor : TextEditor
{
    private WordChecker _wordChecker;
    public const int NumCharsPerLine = 5;

    private void Awake()
    {
        _wordChecker = FindObjectOfType<WordChecker>();
    }

    public List<int> GetFullIndices(int lineIndex)
    {
        var line = GetLine(lineIndex);
        var full = new List<int>();
        for (int i = 0; i < line.Length; i++)
        {
            full.Add(i);
        }
        //full = (List<int>)Enumerable.Range(0, line.Length);
        return full;
    }
    
    public List<int> GetEmptyIndices(int lineIndex)
    {
        var line = GetLine(lineIndex);
        var empty = new List<int>();
        for (int i = line.Length; i < NumCharsPerLine; i++)
        {
            empty.Add(i);
        }
        return empty;
    }

    protected override bool IsInputValid(char character)
    {
        var lines = base.GetLines();
        var finalLine = lines[^1];
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
        else if (character is >= 'a' and <= 'z')
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
