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
    
    protected virtual void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
        TilemapAnimator = GetComponent<TilemapAnimator>();
        HoverableTilemap = GetComponent<HoverableTilemap>();
    }

    protected virtual void OnEnable()
    {
        MouseInput.OnCursorClick += Click;
    }
    
    protected virtual void OnDisable()
    {
        MouseInput.OnCursorClick -= Click;
    }

    protected abstract void Click(Vector3 worldPosition);
}