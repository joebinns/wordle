using UnityEngine;
using UnityEngine.Tilemaps;

public class TextManager : MonoBehaviour
{
    private Vector3Int _gridIndex = Vector3Int.zero;
    [SerializeField] private Tile _tile;

    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void DeleteTextAtCurrentIndex()
    {
        if (_gridIndex.x > 0 & _gridIndex.x <= FindObjectOfType<Grid>().Dimensions.x)
        {
            _gridIndex.x--;
            DeleteCharacter(_gridIndex);
        }
    }
    
    public void SetTextAtCurrentIndex(char character)
    {
        if (_gridIndex.x >= 0 & _gridIndex.x < FindObjectOfType<Grid>().Dimensions.x)
        {
            SetCharacter(_gridIndex, character);
            _gridIndex.x++;
        }
    }
    
    private void SetCharacter(Vector3Int position, char character)
    {
        var tile = FindTileByCharacter(character);
        _tilemap.SetTile(position, tile);
    }
    
    private void DeleteCharacter(Vector3Int position)
    {
        _tilemap.SetTile(position, null);
    }

    private Tile FindTileByCharacter(char character) => Resources.Load(character.ToString()) as Tile;
}
