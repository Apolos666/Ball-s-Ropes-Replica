using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    public float GravityScale = 1f;

    public static float GlobalGravity = -9.81f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        Vector3 gravity = Vector3.up * (GlobalGravity * GravityScale);
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
}
