using UnityEngine;

public class WordChecker : MonoBehaviour
{
    private string _word;
    private Dictionary _dictionary;

    private void Awake()
    {
        _dictionary = FindObjectOfType<Dictionary>();
        _word = _dictionary.GetRandomWord();
        Debug.Log(_word);
    }

    public bool IsWordRecognised(string word)
    {
        return _dictionary.Contains(word);
    }

    public bool IsCharacterIncluded(char character)
    {
        return _word.Contains(character);
    }

    public bool IsCharacterIncludedAtIndex(char character, int index)
    {
        return character == _word[index];
    }

    public bool DoesWordMatch(string word)
    {
        return word == _word;
    }
}
