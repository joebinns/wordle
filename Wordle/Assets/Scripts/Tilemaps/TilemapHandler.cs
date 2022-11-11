using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public abstract class TilemapHandler : MonoBehaviour
    {
        private Tilemap _tilemap;
        public Tilemap Tilemap => _tilemap;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGameReset += Reset;
        }
    
        private void OnDisable()
        {
            GameManager.Instance.OnGameReset -= Reset;
        }

        private void Reset()
        {
            ResetTilemap();
        }

        private void ResetTilemap()
        {
            for (int x = 0; x < _tilemap.size.x; x++)
            {
                for (int y = 0; y < _tilemap.size.y; y++)
                {
                    for (int z = 0; z < _tilemap.size.z; z++)
                    {
                        var position = new Vector3Int(x, -y, 0);
                        ResetTile(position);
                    }
                }
            }
        }

        protected abstract void ResetTile(Vector3Int position);
    }
}
