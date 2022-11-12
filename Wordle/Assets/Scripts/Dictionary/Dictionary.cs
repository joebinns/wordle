using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dictionary : MonoBehaviour
{
    private List<string> _dictionary;
    
    private void Awake()
    {
        var textAsset = Resources.Load("easy_five_letter_dictionary") as TextAsset;
        _dictionary = new List<string>(textAsset.text.Split('\n'));
        for (int i = 0; i < _dictionary.Count; i++)
        {
            _dictionary[i] = _dictionary[i].Trim();
        }
    }

    public bool Contains(string word)
    {
        return _dictionary.Contains(word);
    }

    public string GetRandomWord()
    {
        var index = Random.Range(0, _dictionary.Count);
        return _dictionary[index];
    }
}
