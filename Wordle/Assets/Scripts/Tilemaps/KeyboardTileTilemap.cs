using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class KeyboardTileTilemap : MonoBehaviour
{
    [SerializeField] private Color correct_guess;
    [SerializeField] private Color semi_correct_guess;
    [SerializeField] private Color wrong_guess;
    [SerializeField] private Color un_guessed;
    
    [HideInInspector] public Dictionary<Vector3Int, TileState> PositionToTileState = new Dictionary<Vector3Int, TileState>();
    
    public Tilemap Tilemap;
    private KeyboardTilemap _keyboardTilemap;
    

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        _keyboardTilemap = FindObjectOfType<KeyboardTilemap>();
        ResetTilemap();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }

    public Vector3Int TileNameToPosition(string name)
    {
        return _keyboardTilemap.TileNameToPosition[name];
    }
    
    /*
    public void SetColor(string name, TileState tileState)
    {
        var color = TileStateToColor(tileState);
        var position = TileNameToPosition(name);
        PositionToTileState[position] = tileState;
        SetColor(position, color);
    }
    */
    
    public void SetColor(Vector3Int position, TileState tileState)
    {
        var color = TileStateToColor(tileState);
        PositionToTileState[position] = tileState;
        SetColor(position, color);
    }
    
    private void SetColor(Vector3Int position, Color color)
    {
        Tilemap.SetTileFlags(position, TileFlags.None);
        Tilemap.SetColor(position, color);
        Tilemap.SetTileFlags(position, TileFlags.LockColor);
        //_tilemap.RefreshTile(position);
    }

    private Color TileStateToColor(TileState tileState)
    {
        var color = Color.black;
        switch (tileState)
        {
            case TileState.CorrectGuess:
                color = correct_guess;
                break;
            case TileState.SemiCorrectGuess:
                color = semi_correct_guess;
                break;
            case TileState.WrongGuess:
                color = wrong_guess;
                break;
            case TileState.UnGuessed:
                color = un_guessed;
                break;
        }
        return color;
    }

    private void Reset()
    {
        ResetTilemap();
    }

    private void ResetTilemap()
    {
        for (int x = 0; x < Tilemap.size.x; x++)
        {
            for (int y = 0; y < Tilemap.size.y; y++)
            {
                for (int z = 0; z < Tilemap.size.z; z++)
                {
                    var position = new Vector3Int(x, -y, 0);
                    SetColor(position, TileState.UnGuessed);
                }
            }
        }
    }
}
