using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapAnimator))]
[RequireComponent(typeof(HoverableTilemap))]
public abstract class ClickableTilemap : MonoBehaviour
{
    protected Tilemap Tilemap;
    protected TilemapAnimator TilemapAnimator;
    protected HoverableTilemap HoverableTilemap;
    
    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        TilemapAnimator = GetComponent<TilemapAnimator>();
        HoverableTilemap = GetComponent<HoverableTilemap>();
    }

    private void OnEnable()
    {
        MouseInput.OnCursorClick += Click;
    }
    
    private void OnDisable()
    {
        MouseInput.OnCursorClick -= Click;
    }

    protected abstract void Click(Vector3 worldPosition);
}