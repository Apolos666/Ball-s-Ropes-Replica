using System;
using Apolos.Core;
using UnityEngine;

public class SnapAttractor : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Draggable _draggable;
    
    private Vector3 _previousAttractPoint = Vector3.zero;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attract Point"))
        {
            _previousAttractPoint = other.transform.position;
        }
    }

    public void OnPointerUp()
    {
        if (_previousAttractPoint == Vector3.zero) return;
        _draggable.StopMoving();

        _parent.position = _previousAttractPoint;
    }
}
