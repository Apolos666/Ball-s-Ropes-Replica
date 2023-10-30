using System;
using Apolos.Core;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectOverlapPrevent : MonoBehaviour
{
    [SerializeField] private Transform _ropePointTransform;
    private Vector3 _closestPoint;
    private Vector3 _snapDir;
    [SerializeField] private float _configNumber = 0.15f;
    [SerializeField] private Draggable _draggable;
    private bool _isOnTriggerStay;
    private bool _isGOMoving;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_closestPoint, 0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isGOMoving)
        {
            if (Vector3.Distance(other.ClosestPoint(_ropePointTransform.position), transform.position) == 0)
            {
                return;
            }
            _closestPoint =  other.ClosestPoint(_ropePointTransform.position);
            _snapDir = (_closestPoint - _ropePointTransform.position).normalized;
        }

        _isOnTriggerStay = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isOnTriggerStay = false;
    }

    public void OnPointerUp()
    {
        if (_isGOMoving && _isOnTriggerStay)
        {
            _draggable.StopMoving();
            _ropePointTransform.position = _closestPoint;
            _ropePointTransform.position -= _snapDir * _configNumber;
        }

        _isGOMoving = false;    
    }

    public void OnDrag()
    {
        _isGOMoving = true;
    }
}
