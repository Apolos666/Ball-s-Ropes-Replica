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
    private bool _isOnTriggerEnter;
    private bool _isGOMoving;

    private void OnTriggerEnter(Collider other)
    {
        if (_isGOMoving)
        {
            print("On Trigger Enter");
            _closestPoint =  other.ClosestPoint(_ropePointTransform.position);
            _snapDir = (_closestPoint - _ropePointTransform.position).normalized;
        }

        _isOnTriggerEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        print("On Trigger Exit");
        _isOnTriggerEnter = false;
    }

    public void OnPointerUp()
    {
        if (_isGOMoving && _isOnTriggerEnter)
        {
            print("On Pointer Up");
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
