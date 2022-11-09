using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
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
    private Dictionary<string, Vector3Int> _tileNameToPosition = new Dictionary<string, Vector3Int>();

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _grid = FindObjectOfType<Grid>();
        _tileNameToPosition = MapTileNamesToPositions();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameReset += Reset;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= Reset;
    }
    
    public void SetColor(string name, TileState tileState)
    {
        var color = TileStateToColor(tileState);
        SetColor(_tileNameToPosition[name], color);
    }
    
    private void SetColor(Vector3Int position, TileState tileState)
    {
        var color = TileStateToColor(tileState);
        SetColor(position, color);
    }
    
    private void SetColor(Vector3Int position, Color color)
    {
        _tilemap.SetTileFlags(position, TileFlags.None);
        
        // TODO: Create vector image type tile?
        //_tilemap.SetColor(position, color); 
        // https://answers.unity.com/questions/1686322/vector-graphics-package-change-sprite-color.html
        // https://forum.unity.com/threads/vector-graphics-preview-package.529845/page-2#post-3522208

        //var tileBase = _tilemap.GetTile(position);
        //var tile = tileBase as Tile;
        //tile.sprite.GetComponent<SVGImage>().color = color;

        _tilemap.SetTileFlags(position, TileFlags.LockColor);
        _tilemap.RefreshTile(position);
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
    
    /*
    // TODO: Create a function to find the position of a tile in the tilemap by name.
    private void GetUsedTiles()
    {
        TileBase[] usedTiles = new TileBase[26];
        _tilemap.GetUsedTilesNonAlloc(usedTiles);

        string[] names = new string[26];
        for (int i = 0; i < usedTiles.Length; i++)
        {
            names[i] = usedTiles[i].name;
        }

        // TODO: Create dictionary which maps usedTilesNames to position
        
    }
    */

    private Dictionary<string, Vector3Int> MapTileNamesToPositions()
    {
        var tileNameToPosition = new Dictionary<string, Vector3Int>();
        for (int x = 0; x < _tilemap.size.x; x++)
        {
            for (int y = 0; y < _tilemap.size.y; y++)
            {
                for (int z = 0; z < _tilemap.size.z; z++)
                {
                    var position = new Vector3Int(x, -y, z);
                    var tile = _tilemap.GetTile(position);
                    if (tile == null) { continue; }
                    var name = tile.name;
                    tileNameToPosition[name] = position;
                }
            }
        }
        return tileNameToPosition;
    }
}
