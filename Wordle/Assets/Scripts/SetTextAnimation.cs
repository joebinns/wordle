using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTextAnimation : Animation
{
    public override IEnumerator Play(Context context)
    {
        /*
        var characterIndex = _wordleTextEditor.GetFinalLine().Length - 1;
        var characterPosition = IndexToPosition(characterIndex);
        if (character is >= 'a' and <= 'z')
        {
            var tile = TilemapUtilities.FindTileByCharacter(character);
            _guessesAnimations.SetLetter(characterPosition, tile);
        }
        else if (character == '\b')
        {
            characterPosition.x++;
            _guessesAnimations.SetLetter(characterPosition, null);
        }
        else if (character == '\r')
        {
            var word = _wordleTextEditor.GetLine(_wordleTextEditor.GetFinalLineIndex() - 1);
            var indexToTileState = _wordChecker.GetTileStates(word);
            var indices = indexToTileState.Keys.ToList();
            var positionToTile = new Dictionary<Vector3Int, Tile>();
            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                var position = IndexToPosition(index);
                position.y++;
                var tile = _guessesDecoratorTilemapHandler.TileStateToTile(indexToTileState[index]);
                positionToTile[position] = tile;
            }
            _guessesAnimations.RevealGuessTiles(positionToTile);
        }
        */
        yield return null;
    }
}
