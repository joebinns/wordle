using System.Collections;
using Audio;
using UnityEngine;

public class SetTextAnimation : Animation
{
    [SerializeField] private TilemapAnimator _letterTilemapAnimator;
    
    private WordleTextEditor _wordleTextEditor;

    private void Awake()
    {
        _wordleTextEditor = FindObjectOfType<WordleTextEditor>();
    }

    public override IEnumerator Play(Context context)
    {
        var character = context.Character;
        var characterIndex = _wordleTextEditor.GetLine(context.LineIndex).Length - 1;
        var characterPosition = IndexToPosition(characterIndex);
        if (character is >= 'a' and <= 'z')
        {
            var tile = TilemapUtilities.FindTileByCharacter(character);
            _letterTilemapAnimator.SetTile(characterPosition, tile);
            AudioManager.Instance.Play("Keyboard Primary");
        }
        else if (character == '\b')
        {
            characterPosition.x++;
            _letterTilemapAnimator.SetTile(characterPosition, null);
            AudioManager.Instance.Play("Keyboard Secondary");
        }
        else if (character == '\r')
        {
            AudioManager.Instance.Play("Keyboard Secondary");
        }
        yield return null;
    }
    
    private Vector3Int IndexToPosition(int index)
    {
        var lineIndex = _wordleTextEditor.GetFinalLineIndex();
        return new Vector3Int(index, -lineIndex, 0);
    }
}
