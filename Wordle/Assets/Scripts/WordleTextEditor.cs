using System;
using System.Collections;

public class WordleTextEditor : TextEditor
{
    private WordChecker _wordChecker;
    private const int NUM_CHARS_PER_LINE = 5;
    
    private void Awake()
    {
        _wordChecker = FindObjectOfType<WordChecker>();
    }
    
    protected override bool IsInputValid(char character)
    {
        var lines = Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var finalLine = lines[^1];
        if (character == '\n')
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
            var isLineFull = finalLine.Length == NUM_CHARS_PER_LINE;
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
        var isLineFull = line.Length == NUM_CHARS_PER_LINE;
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
