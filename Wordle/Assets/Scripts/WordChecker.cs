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

    private void Reset()
    {
        _word = _dictionary.GetRandomWord();
//        Debug.Log(_word);
    }
}
