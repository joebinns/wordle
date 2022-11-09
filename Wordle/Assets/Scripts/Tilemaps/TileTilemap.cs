using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTilemap : MonoBehaviour
{
    private Tile correct_guess;
    private Tile semi_correct_guess;
    private Tile wrong_guess;
    private Tile un_guessed;

    private Grid _grid;
    private Tilemap _tilemap;

    private void Awake()
    {
        GetTilesFromResources();
        _tilemap = GetComponent<Tilemap>();
        _grid = FindObjectOfType<Grid>();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }

    public void SetTile(Vector3Int position, TileState tileState)
    {
        var tile = TileStateToTile(tileState);
        SetTile(position, tile);
    }
    
    private void SetTile(Vector3Int position, Tile tile)
    {
        _tilemap.SetTile(position, tile);
    }

    private Tile TileStateToTile(TileState tileState)
    {
        Tile tile = null;
        switch (tileState)
        {
            case TileState.CorrectGuess:
                tile = correct_guess;
                break;
            case TileState.SemiCorrectGuess:
                tile = semi_correct_guess;
                break;
            case TileState.WrongGuess:
                tile = wrong_guess;
                break;
            case TileState.UnGuessed:
                tile = un_guessed;
                break;
        }
        return tile;
    }

    private void GetTilesFromResources()
    {
        correct_guess = Resources.Load("correct_guess") as Tile;
        semi_correct_guess = Resources.Load("semi_correct_guess") as Tile;
        wrong_guess = Resources.Load("wrong_guess") as Tile;
        un_guessed = Resources.Load("un_guessed") as Tile;
    }

    private void Reset()
    {
        ResetTilemap();
    }

    private void ResetTilemap()
    {
        for (int x = 0; x < _grid.Dimensions.x; x++)
        {
            for (int y = 0; y < _grid.Dimensions.y; y++)
            {
                var position = new Vector3Int(x, -y, 0);
                SetTile(position, TileState.UnGuessed);
            }
        }
    }
}

public enum TileState
{
    UnGuessed = 0,
    WrongGuess = 1,
    SemiCorrectGuess = 2,
    CorrectGuess = 3
}
