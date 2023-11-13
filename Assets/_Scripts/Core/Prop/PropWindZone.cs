using System;
using Apolos.Core;
using Apolos.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PropWindZone : MonoBehaviour
{
    #region Comment

    // [FormerlySerializedAs("boxSize")] 
    // [SerializeField] private Vector3 _boxSize;
    //
    // [FormerlySerializedAs("boxCenter")] 
    // [SerializeField] private Transform _boxCenter;
    //
    // [FormerlySerializedAs("offset")] 
    // [SerializeField] private Vector3 _offset;
    //
    // [SerializeField] [ReadOnly] private LayerMask _windZoneAllowedLayers;
    // [SerializeField] private int _collidersSize = 20;
    //
    // [SerializeField] private Collider[] _colliders;
    // private int _amountCollInWindZone;

    // private void Awake()
    // {
    //     _colliders = new Collider[_collidersSize];
    // }
    //
    // private void Update()
    // {
    //     WindForce();
    // }
    //
    // private void WindForce()
    // {
    //     if (!IsAnyEnterWindZone(out _amountCollInWindZone)) return;
    //
    //     for (int i = 0; i < _amountCollInWindZone; i++)
    //     {
    //         
    //     }
    // }
    //
    // private bool IsAnyEnterWindZone(out int amountCollInWindZone)
    // {
    //     Array.Clear(_colliders, 0, _colliders.Length);
    //
    //     amountCollInWindZone = Physics.OverlapBoxNonAlloc(_boxCenter.position + _offset, _boxSize / 2, _colliders,
    //         Quaternion.identity, _windZoneAllowedLayers);
    //
    //     return amountCollInWindZone > 0;
    // }
    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(_boxCenter.position + _offset, _boxSize);
    // }

    #endregion

    [SerializeField] private float _forceMultiply = 100f;
    [SerializeField] private PointEventChannelSO _onGetPoint;
    [SerializeField] private Transform _pointBlowUp;
    
    private void OnTriggerStay(Collider other)
    {
        print("Hello 1");
        if (other.TryGetComponent<Rigidbody>(out var rb))
        {
            print("Hello");
            rb.AddForce(Vector3.up * _forceMultiply * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rb))
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                var point = ball.Point;
                _onGetPoint.RaiseEvent(point, _pointBlowUp.position);
            }
        }
    }
}