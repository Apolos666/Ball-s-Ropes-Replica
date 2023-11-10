using System;
using System.Collections;
using System.Collections.Generic;
using Apolos.Core;
using Obi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class RopeCollisionHandler : MonoBehaviour
{
    private ObiSolver _obiSolver;
    private Rigidbody _currentBallrb;
    private ObiSolver.ParticleInActor _currentActor;
    private ObiRope _colliderObiRope;
    [SerializeField] private float _forceMultipleFirstCollision;

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
            if (contact.distance < 0.001f)
            {
                var obiCollider = GetColliderBasedOnContact(contact);
                if (obiCollider != null)
                {
                    _currentBallrb = obiCollider.GetComponent<Rigidbody>();
                    ApplyForceToCollider(contact);
                }
            }
        }
    }

    private void ApplyForceToCollider(Oni.Contact contact)
    {
        var inDir = _currentBallrb.velocity;
        var normal = new Vector3(-contact.normal.x, -contact.normal.y, contact.normal.z);

        var reflectForce = Vector3.Reflect(inDir, normal);

        ApplyForceCondition(reflectForce);
    }

    private void ApplyForceCondition(Vector3 reflectForce)
    {
        _currentBallrb.velocity = reflectForce * (_forceMultipleFirstCollision * Time.fixedDeltaTime);
    }  

    private ObiColliderBase GetColliderBasedOnContact(Oni.Contact contact)
    {
        var obiCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
        return obiCollider;
    }
}