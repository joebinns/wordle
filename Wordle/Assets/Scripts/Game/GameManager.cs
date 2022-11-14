using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public event Action OnGameReset;

    private bool _isReseting;

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
        if (_isReseting) { return; }
        StartCoroutine(ResetGameDelayed(delay));
    }

    private IEnumerator ResetGameDelayed(float delay)
    {
        _isReseting = true;
        yield return new WaitForSeconds(delay);
        OnGameReset?.Invoke();
        _isReseting = false;
    }
}
