using System;
using System.Collections.Generic;
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
    private bool _isGOMoving;
    private bool _firstEnterTrigger = true;

    private List<Collider> _triggerColliders;

    private void Awake()
    {
        _triggerColliders = new List<Collider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_closestPoint, 0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_triggerColliders.Contains(other))
        {
            _triggerColliders.Add(other);
        }
        
        if (_isGOMoving)
        {
            if (_triggerColliders.Count > 0 && !_firstEnterTrigger) return;
            
            if (IsInSideAnotherObject(other)) return;
            
            _closestPoint =  other.ClosestPoint(_ropePointTransform.position);
            _snapDir = (_closestPoint - _ropePointTransform.position).normalized;
            _firstEnterTrigger = false;
        }
    }

    private bool IsInSideAnotherObject(Collider other)
    {
        return Vector3.Distance(other.ClosestPoint(_ropePointTransform.position), transform.position) == 0;
    }

    private void OnTriggerExit(Collider other)
    {
        print(other.gameObject.name);
        
        if (_triggerColliders.Contains(other))
        {
            _triggerColliders.Remove(other);
        }
        _firstEnterTrigger = true;
    }

    private void Update()
    {
        if (_isGOMoving)
        {
            print(_triggerColliders.Count);
        }
    }

    public void OnPointerUp()
    {
        if (_isGOMoving && _triggerColliders.Count > 0)
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
