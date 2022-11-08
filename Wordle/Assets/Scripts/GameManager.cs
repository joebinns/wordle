using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public event Action OnGameReset;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void ResetGame(float delay = 1f)
    {
        StartCoroutine(ResetGameDelayed(delay));
    }

    private IEnumerator ResetGameDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameReset?.Invoke();
    }
}
