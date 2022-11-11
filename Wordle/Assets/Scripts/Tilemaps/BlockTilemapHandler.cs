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

        public void ApplyColorOverlay(Vector3Int position, Color overlayColor)
        {
            if (!Tilemap.GetTile(position)) { return; }
            var color = TileStateToTile(_positionToTileState[position]).color;
            color *= overlayColor;
            SetColor(position, color);
        }

        public void ResetColorOverlay(Vector3Int position)
        {
            if (!Tilemap.GetTile(position)) { return; }
            var color = TileStateToTile(_positionToTileState[position]).color;
            SetColor(position, color);
        }
    
        private void SetColor(Vector3Int position, Color color)
        {
            Tilemap.SetTileFlags(position, TileFlags.None);
            Tilemap.SetColor(position, color);
            Tilemap.SetTileFlags(position, TileFlags.LockColor);
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
        
        protected override void ResetTile(Vector3Int position)
        {
            SetTileState(position, TileState.UnGuessed);
        }
    }
}