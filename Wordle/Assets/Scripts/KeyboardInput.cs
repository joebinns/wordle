using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    private TextTilemap _textTilemap;

    private void Awake()
    {
        _textTilemap = FindObjectOfType<TextTilemap>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if ((c == '\n') || (c == '\r')) // enter/return
                {
                    _textTilemap.EnterText();
                }
                
                else
                {
                    _textTilemap.SetCharacterAtCaret(c);
                }
            }
        }
    }
}
