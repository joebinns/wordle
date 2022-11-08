using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyboardTilemap : MonoBehaviour
{
    [SerializeField] private Color correct_guess;
    [SerializeField] private Color semi_correct_guess;
    [SerializeField] private Color wrong_guess;
    [SerializeField] private Color un_guessed;

    private Grid _grid;
    private Tilemap _tilemap;

    private void Awake()
    {
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

    public void SetColor(Vector3Int position, TileState tileState)
    {
        var color = TileStateToColor(tileState);
        SetColor(position, color);
    }
    
    private void SetColor(Vector3Int position, Color color)
    {
        _tilemap.SetColor(position, color);
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
        for (int x = 0; x < _grid.Dimensions.x; x++)
        {
            for (int y = 0; y < _grid.Dimensions.y; y++)
            {
                var position = new Vector3Int(x, -y, 0);
                SetColor(position, TileState.UnGuessed);
            }
        }
    }
    
    // TODO: Create a function to find the position of a tile in the tilemap by name.
}
