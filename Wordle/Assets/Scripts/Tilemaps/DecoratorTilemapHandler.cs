using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class DecoratorTilemapHandler : TilemapHandler
    {
        // TileState
        
        public Tile correct_guess;
        [SerializeField] private Tile semi_correct_guess;
        [SerializeField] private Tile wrong_guess;
        [SerializeField] private Tile un_guessed;
        
        private Dictionary<Vector3Int, TileState> _positionToTileState = new Dictionary<Vector3Int, TileState>();
        public Dictionary<Vector3Int, TileState> PositionToTileState => _positionToTileState;
        
        private Dictionary<TileState, Tile> _tileStateToTile = new Dictionary<TileState, Tile>();

        public Tile TileStateToTile(TileState tileState) { return _tileStateToTile[tileState]; }
        public TileState TileToTileState(Tile tile) { return _tileStateToTile.FirstOrDefault(x => x.Value == tile).Key; }
        
        protected override void Awake()
        {
            base.Awake();
            InitialisePositionsToTileStates();
            InitialiseTileStateToTile();
        }
        
        private void InitialisePositionsToTileStates()
        {
            for (int x = 0; x < Tilemap.size.x; x++)
            {
                for (int y = 0; y < Tilemap.size.y; y++)
                {
                    for (int z = 0; z < Tilemap.size.z; z++)
                    {
                        var position = new Vector3Int(x, -y, z);
                        if (!Tilemap.HasTile(position)) { continue; }
                        _positionToTileState[position] = TileState.UnGuessed;
                    }
                }
            }
        }
        
        private void InitialiseTileStateToTile()
        {
            _tileStateToTile[TileState.CorrectGuess] = correct_guess;
            _tileStateToTile[TileState.SemiCorrectGuess] = semi_correct_guess;
            _tileStateToTile[TileState.WrongGuess] = wrong_guess;
            _tileStateToTile[TileState.UnGuessed] = un_guessed;
        }

        public void SetTile(Vector3Int position, Tile tile)
        {
            var tileState = TileToTileState(tile);
            SetTileState(position, tileState);
        }

        public void SetTileDelayed(Vector3Int position, Tile tile, float delay)
        {
            var tileState = TileToTileState(tile);
            SetTileStateDelayed(position, tileState, delay);
        }
        
        public void SetTileStateCautious(Vector3Int position, TileState tileState)
        {
            if (tileState > _positionToTileState[position])
            {
                SetTileState(position, tileState);
            }
        }
        
        public void SetTileState(Vector3Int position, TileState tileState)
        {
            _positionToTileState[position] = tileState;
            var tile = TileStateToTile(tileState);
            Tilemap.SetTile(position, tile);
        }

        public void SetTileStateDelayed(Vector3Int position, TileState tileState, float delay)
        {
            StartCoroutine(SetTileStateDelayedCoroutine(position, tileState, delay));
        }

        private IEnumerator SetTileStateDelayedCoroutine(Vector3Int position, TileState tileState, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetTileState(position, tileState);
        }

        protected override void ResetTile(Vector3Int position)
        {
            /*
            if (Tilemap.HasTile(position))
            {
                SetTileState(position, TileState.UnGuessed);
            }
            */
        }
    }
}