using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public abstract class ClickableTilemap : MonoBehaviour
{
    protected Tilemap Tilemap;
    
    protected virtual void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
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