using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private Vector2 _previousCursorHitPoint;
    public static event Action<Vector3> OnCursorPositionChanged;
    public static event Action<Vector3> OnCursorClick;

    private void Update()
    {
        UpdateCursorClick(UpdateCursorHitPoint());
    }

    private Vector2 UpdateCursorHitPoint()
    {
        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        var cursorHitPoint = rayHit.collider != null ? rayHit.point : -Vector2.one;
        if (cursorHitPoint != _previousCursorHitPoint)
        {
            OnCursorPositionChanged?.Invoke(cursorHitPoint);
        }
        _previousCursorHitPoint = cursorHitPoint;
        return cursorHitPoint;
    }

    private bool UpdateCursorClick(Vector2 cursorHitPoint)
    {
        var didCursorClick = Input.GetMouseButtonDown(0);
        if (didCursorClick)
        {
            OnCursorClick?.Invoke(cursorHitPoint);
        }
        return didCursorClick;
    }
}
