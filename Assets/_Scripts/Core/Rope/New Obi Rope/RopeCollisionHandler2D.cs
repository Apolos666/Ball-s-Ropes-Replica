using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class RopeCollisionHandler2D : MonoBehaviour
{
    [Header("Force Modifier")] 
    [SerializeField] private float _forceMutilple;
    
    // Collision Refs
    private ObiSolver _obiSolver;
    private ObiSolver.ParticleInActor _currentActor;
    private Rigidbody2D _rb2D;
    
    // forces
    private Vector2 _inDirForce;
    private Vector2 _normalRope;
    private Vector2 _reflectForce;
    private Vector2 _contactPoint;

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
    
    private void OnCollisionHandler(ObiSolver solver, ObiSolver.ObiCollisionEventArgs contacts)
    {
        foreach (var contact in contacts.contacts)
        {
            if (contact.distance < 0.01f)
            {
                var obiCollider2D = GetColliderBasedOnContact(contact);

                if (obiCollider2D != null)
                {
                    _currentActor = GetCurrentActor(solver, contact);
                    _rb2D = obiCollider2D.GetComponent<Rigidbody2D>();
                    ApplyForceToRB2D(contact, _currentActor, solver);
                }
                
                break;
            }
        }
    }
    
    private static ObiSolver.ParticleInActor GetCurrentActor(ObiSolver solver, Oni.Contact contact)
    {
        int simplexStart = solver.simplexCounts.GetSimplexStartAndSize(contact.bodyA, out int simplexSize);

        ObiSolver.ParticleInActor actor = null;
        
        for (int i = 0; i < simplexSize; ++i)
        {
            int particleIndex = solver.simplices[simplexStart + i];

            actor = solver.particleToActor[particleIndex];
        }
        
        return actor;
    }

    private void ApplyForceToRB2D(Oni.Contact contact, ObiSolver.ParticleInActor actor, ObiSolver solver)
    {
        _inDirForce = _rb2D.velocity;

        if (actor != null && actor.actor.TryGetComponent<RopeController2D>(out var ropeController2D))
        {
            _normalRope = ropeController2D.GetNormalRope();
            Helper.Vector.VectorsSameDirection(_inDirForce, ref _normalRope);
            _reflectForce = Vector2.Reflect(_inDirForce, _normalRope);

            #region Draw Line

            Vector3 pointBWorld = contact.pointB;
            Vector3 pointBUnity = solver.transform.TransformPoint(pointBWorld);

            _contactPoint = pointBUnity;

            #endregion
        }

        ApplyForceCondition(_reflectForce);
    }


    private void ApplyForceCondition(Vector2 reflectForce)  
    {
        _rb2D.velocity = reflectForce * (_forceMutilple * Time.fixedDeltaTime);
    }  
    
    private ObiColliderBase GetColliderBasedOnContact(Oni.Contact contact)
    {
        var obiCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
        return obiCollider;
    }
    
    private void OnDrawGizmos()
    {
        Debug.DrawLine(_contactPoint, _contactPoint + _inDirForce, Color.red);
        Debug.DrawLine(_contactPoint, _contactPoint + _normalRope, Color.blue);
        Debug.DrawLine(_contactPoint, _contactPoint + _reflectForce, Color.green);
    }
}
