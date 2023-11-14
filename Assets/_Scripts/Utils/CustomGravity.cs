using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField] private float _gravityScale = 1f;

    public static float GlobalGravity = -9.81f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        Vector3 gravity = Vector3.up * (GlobalGravity * _gravityScale);
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }

    public void SetGravityScale(float gravityScale)
    {
        _gravityScale = gravityScale;
    }
}
