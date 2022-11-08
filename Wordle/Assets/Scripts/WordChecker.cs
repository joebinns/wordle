using UnityEngine;

public class WordChecker : MonoBehaviour
{
    private string Word = "slime";

    public bool IsWordRecognised(string word)
    {
        // TODO: Check if word list contains word
        return true;
    }

    public bool IsCharacterIncluded(char character)
    {
        return Word.Contains(character);
    }

    public bool IsCharacterIncludedAtIndex(char character, int index)
    {
        return character == Word[index];
    }

    public bool DoesWordMatch(string word)
    {
        return word == Word;
    }
}
