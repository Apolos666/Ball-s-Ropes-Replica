using System;
using System.Collections;
using System.Collections.Generic;
using Apolos.Core;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Vector3 point2D;
    public bool _useTriggerEnter;
    [SerializeField] private Vector3 dir;
    [SerializeField] private float _configNumber = 3;
    [SerializeField] private Draggable _draggable;
    private bool _isOnTriggerEnter;
    private bool _isGOMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (_isGOMoving)
        {
            print("On Triggger Enter");
            point2D =  other.ClosestPoint(transform.position);
            dir = (point2D - transform.position).normalized;
        }

        _isOnTriggerEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isGOMoving)
        {
            print("On Trigger Exit");
            point2D = Vector3.zero;
        }

        _isOnTriggerEnter = false;
    }

    public void OnPointerUp()
    {
        if (_isGOMoving && _isOnTriggerEnter)
        {
            _draggable.StopMoving();
            transform.position = point2D;
            transform.position -= dir * _configNumber;
        }

        _isGOMoving = false;    
    }

    public void OnDrag()
    {
        _isGOMoving = true;
    }

    private void OnDrawGizmos()
    {
        if (_useTriggerEnter)
        {
            Gizmos.DrawWireSphere(point2D, 0.1f);
        }
    }
}
