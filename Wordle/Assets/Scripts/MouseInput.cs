using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseInput : MonoBehaviour
{
    private TextTilemap _textTilemap;
    private KeyboardTilemap _keyboardTilemap;
    private Vector3Int _previousHoverPosition;
    private List<Vector3Int> _currentlyHoveringPositions = new List<Vector3Int>();
    private List<Vector3Int> _currentlyUnhoveringPositions = new List<Vector3Int>();
    private List<Vector3Int> _currentlyHoveredPositions = new List<Vector3Int>();

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

            // TODO: Scrap this shite and just track the currently hovered position, when it changes (!= previously hovered position), unhover previous and hover new.
            //if (tilemapPosition != _previousHoverPosition)
            if (!_currentlyUnhoveringPositions.Contains(tilemapPosition))
            {
                if (_currentlyHoveredPositions.Contains(_previousHoverPosition))
                {
                    Debug.Log("Unhover");
                    StartCoroutine(UnHoverTile(_previousHoverPosition, tilemap));
                }
            }
            
            if (name != null)
            {
                var character = name[0];

                if (!_currentlyHoveringPositions.Contains(tilemapPosition))
                {
                    if (!_currentlyHoveredPositions.Contains(tilemapPosition))
                    {
                        Debug.Log("Hover");
                        _previousHoverPosition = tilemapPosition;
                        StartCoroutine(HoverTile(tilemapPosition, tilemap));
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    _textTilemap.SetCharacterAtCaret(character);
                }
            }
        }
    }

    private IEnumerator HoverTile(Vector3Int position, Tilemap tilemap)
    {
        _currentlyHoveringPositions.Add(position);
        yield return StartCoroutine(LerpTileScale(position, tilemap, 1.0f, 1.1f));
        _currentlyHoveredPositions.Add(position);
        _currentlyHoveringPositions.Remove(position);
    }

    private IEnumerator UnHoverTile(Vector3Int position, Tilemap tilemap)
    {
        _currentlyHoveredPositions.Remove(position);
        _currentlyUnhoveringPositions.Add(position);
        yield return StartCoroutine(LerpTileScale(position, tilemap, 1.1f, 1.0f));
        _currentlyUnhoveringPositions.Remove(position);
    }
    
    private IEnumerator LerpTileScale(Vector3Int position, Tilemap tilemap, float start, float end)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        var t = 0f;
        var duration = 0.1f;
        while (t < duration)
        {
            var magnitude = start + ((end - start) * t);
            Matrix4x4 newMatrix = Matrix4x4.Scale(Vector3.one * magnitude);
            tilemap.SetTransformMatrix(position, newMatrix);
            t += Time.deltaTime;
            yield return null;
        }
        Matrix4x4 targetMatrix = Matrix4x4.Scale(Vector3.one * end);
        tilemap.SetTransformMatrix(position, targetMatrix);
    }
}
