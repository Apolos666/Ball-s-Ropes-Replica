using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class RopeCollisionHandler : MonoBehaviour
{
    private ObiSolver _obiSolver;
    private Rigidbody _currentBallrb;
    [SerializeField] private float _forceMultiple;

    private void Awake()
    {
        _obiSolver = GetComponent<ObiSolver>();
    }

    private void Start()
    {
        _obiSolver.OnCollision += OnCollisionHandler;
    }

    private void OnDestroy()
    {
        _obiSolver.OnCollision -= OnCollisionHandler;
    }

    private void OnCollisionHandler(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0.0005f)
            {
                var obiCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
                if (obiCollider != null)
                {
                    _currentBallrb = obiCollider.GetComponent<Rigidbody>();

                    var inDir = _currentBallrb.velocity;
                    var normal = new Vector3(-contact.normal.x, -contact.normal.y, contact.normal.z);

                    var reflectForce = Vector3.Reflect(inDir, normal);

                    _currentBallrb.velocity = reflectForce * (_forceMultiple * Time.fixedDeltaTime);
                }
            }
        }
    }
}