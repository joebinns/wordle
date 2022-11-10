using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseInput : MonoBehaviour
{
    private TextTilemap _textTilemap;
    private KeyboardTilemap _keyboardTilemap;

    private void Awake()
    {
        _textTilemap = FindObjectOfType<TextTilemap>();
        _keyboardTilemap = FindObjectOfType<KeyboardTilemap>();
    }

    private void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(rayHit.collider != null)
        {
            var tilemap = rayHit.transform.GetComponent<Tilemap>();
            var tilemapPosition = tilemap.WorldToCell(rayHit.point);
            var name = _keyboardTilemap.PositionToTileName(tilemapPosition);

            if (name != null)
            {
                var character = name[0];
                
                // TODO: Play hover animation
            
                if (Input.GetMouseButtonDown(0))
                {
                    _textTilemap.SetCharacterAtCaret(character);
                }
            }
        }
    }
}
