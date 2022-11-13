using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dictionary : MonoBehaviour
{
    private List<string> _dictionary;
    private IEnumerable<string> _greaterDictionary;
    
    private void Awake()
    {
        _dictionary = ReadTxt("easy_five_letter_dictionary");
        _greaterDictionary = ReadTxt("five_letter_dictionary");
        _greaterDictionary = _dictionary.Union(_greaterDictionary);
    }

    private List<string> ReadTxt(string txt)
    {
        var textAsset = Resources.Load(txt) as TextAsset;
        var list = new List<string>(textAsset.text.Split('\n'));
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].Trim();
        }
        return list;
    }

    public bool Contains(string word)
    {
        return _greaterDictionary.Contains(word);
    }

    public string GetRandomWord()
    {
        var index = Random.Range(0, _dictionary.Count);
        return _dictionary[index];
    }
}
