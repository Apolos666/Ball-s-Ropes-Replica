using System;
using System.Collections.Generic;
using Apolos.Core;
using Apolos.System.EventManager;
using UnityEngine;

public class SnapAttractor : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Draggable _draggable;
    [SerializeField] private float _snapPositionZ = -1f;
    private Collider _collder;
    
    private Vector3 _previousAttractPoint = Vector3.zero;

    private void Awake()
    {
        _collder = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attract Point"))
        {
            if (other.TryGetComponent<Attractor>(out var attractor))
            {
                EventManagerGeneric<Vector3, bool>.RaiseEvent("OnEnterNewAttractPoint", new Dictionary<Vector3, bool> {{_previousAttractPoint, false}});
                var parentPos = attractor.GetTransformParent().position;
                if (GenerateGrid.GetOccupiedDict[parentPos])
                {
                    return;
                }
                _previousAttractPoint = parentPos;
            }
        }
    }

    public void OnDrag()
    {
        _collder.enabled = true;
    }
    
    public void OnPointerUp()
    {
        if (_previousAttractPoint == Vector3.zero) return;
        _draggable.StopMoving();
        _collder.enabled = false;
        EventManagerGeneric<Vector3, bool>.RaiseEvent("OnSpotOccupied", new Dictionary<Vector3, bool>{{_previousAttractPoint, true}});
        _parent.position = new Vector3(_previousAttractPoint.x, _previousAttractPoint.y, _snapPositionZ);
    }
}
