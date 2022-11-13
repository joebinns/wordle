using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    private string _word;
    private Dictionary _dictionary;

    private void Awake()
    {
        _dictionary = FindObjectOfType<Dictionary>();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }

    private void Start()
    {
        Reset();
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

    public Dictionary<char, int> GetCharToExcess(string word)
    {
        var charToNumberDuplicateOccurrencesTarget = GetCharToNumberDuplicateOccurrences(_word);
        var charToNumberDuplicateOccurrencesGuess = GetCharToNumberDuplicateOccurrences(word);
        return GetDeltaCharToNumberDuplicateOccurrences(charToNumberDuplicateOccurrencesTarget, charToNumberDuplicateOccurrencesGuess);
    }
    
    private Dictionary<char, int> GetCharToNumberDuplicateOccurrences(string word)
    {
        var charToNumberDuplicateOccurrences = new Dictionary<char, int>();
        foreach (char character in word)
        {
            charToNumberDuplicateOccurrences[character] = 0;
        }
        foreach (char character in word)
        {
            charToNumberDuplicateOccurrences[character] += 1;
        }

        return charToNumberDuplicateOccurrences;
    }
    
    private Dictionary<char, int> GetDeltaCharToNumberDuplicateOccurrences(Dictionary<char, int> charToNumberDuplicateOccurrencesA, Dictionary<char, int> charToNumberDuplicateOccurrencesB)
    {
        var deltaCharToNumberDuplicateOccurrences = new Dictionary<char, int>();
        
        // Get characters which are in both A and B:
        var charactersA = charToNumberDuplicateOccurrencesA.Keys.ToList();
        var charactersB = charToNumberDuplicateOccurrencesB.Keys.ToList();
        var intersectingCharacters = charactersA.Intersect(charactersB);

        // B - A
        foreach (char character in intersectingCharacters)
        {
            deltaCharToNumberDuplicateOccurrences[character] = charToNumberDuplicateOccurrencesB[character] -
                                                               charToNumberDuplicateOccurrencesA[character];
        }

        return deltaCharToNumberDuplicateOccurrences;
    }

    private void Reset()
    {
        _word = _dictionary.GetRandomWord();
        Debug.Log(_word);
    }
}
