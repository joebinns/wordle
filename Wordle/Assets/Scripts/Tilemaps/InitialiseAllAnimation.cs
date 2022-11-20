using System.Collections;
using UnityEngine;

public class InitialiseAllAnimation : Animation
{
    [SerializeField] private TilemapAnimator _decoratorTilemapAnimator;
    
    public override IEnumerator Play()
    {
        for (int x = 0; x < WordleTextEditor.NumCharsPerLine; x++)
        {
            for (int y = 0; y < WordleTextEditor.MaxNumLines + 1; y++)
            {
                var position = new Vector3Int(x, -y, 0);
                _decoratorTilemapAnimator.SetRotation(position, Vector3.right * -90f);
            }
        }

        yield return null;
    }
}