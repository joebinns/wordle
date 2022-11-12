using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tilemaps
{
    public class BlockTilemapHandler : TilemapHandler
    {
        // TileState
        
        [SerializeField] private Tile correct_guess;
        [SerializeField] private Tile semi_correct_guess;
        [SerializeField] private Tile wrong_guess;
        [SerializeField] private Tile un_guessed;
        
        private Dictionary<Vector3Int, TileState> _positionToTileState = new Dictionary<Vector3Int, TileState>();

        protected override void Awake()
        {
            base.Awake();
            _positionToTileState = InitialisePositionsToTileStates();
        }
        
        private Dictionary<Vector3Int, TileState> InitialisePositionsToTileStates()
        {
            var positionToTileState = new Dictionary<Vector3Int, TileState>();
            for (int x = 0; x < Tilemap.size.x; x++)
            {
                for (int y = 0; y < Tilemap.size.y; y++)
                {
                    for (int z = 0; z < Tilemap.size.z; z++)
                    {
                        var position = new Vector3Int(x, -y, z);
                        if (!Tilemap.HasTile(position)) { continue; }
                        positionToTileState[position] = TileState.UnGuessed;
                    }
                }
            }
            return positionToTileState;
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
        
        public Tile TileStateToTile(TileState tileState)
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
        
        protected override void ResetTile(Vector3Int position)
        {
            if (Tilemap.HasTile(position))
            {
                SetTileState(position, TileState.UnGuessed);
            }
        }
    }
}