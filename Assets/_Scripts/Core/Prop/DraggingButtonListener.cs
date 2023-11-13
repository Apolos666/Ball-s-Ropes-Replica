using System;
using UnityEngine;

public class DraggingButtonListener : MonoBehaviour
{
    [SerializeField] private DraggingButtonAroundCircle _events;

    private void OnEnable()
    {
        _events.OnDragging += OnDragging;
    }

    private void OnDisable()
    {
        _events.OnDragging -= OnDragging;
    }

    private void OnDragging(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
