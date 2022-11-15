using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public event Action OnGameReset;

    private bool _isResetting;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void ResetGame(float delay = 0f)
    {
        if (_isResetting) { return; }
        StartCoroutine(ResetGameDelayed(delay));
    }

    private IEnumerator ResetGameDelayed(float delay)
    {
        _isResetting = true;
        yield return new WaitForSeconds(delay);
        OnGameReset?.Invoke();
        _isResetting = false;
    }
}
