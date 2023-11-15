using System;
using Apolos.Core;
using Apolos.SO;
using Apolos.System;
using Obi;
using UnityEngine;
using UnityEngine.Serialization;

public class RopeCollisionHandler : MonoBehaviour
{
    private ObiSolver _obiSolver;
    private Rigidbody _currentBallrb;
    private ObiSolver.ParticleInActor _currentActor;
    private ObiRope _colliderObiRope;
    [SerializeField] private float _forceMultipleFirstCollision;
    [SerializeField] private PointEventChannelSO _contactPointEvent;
    [SerializeField] private AudioClip _contactPointClip;

    #region  "Force"

    private Vector3 _inDirForce = Vector3.zero;
    private Vector3 _normal = Vector3.zero;
    private Vector3 _reflectForce = Vector3.zero;
    private Vector3 _contactPoint = Vector3.zero;
    private float _forceAmount = 0f;
    
    #endregion

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
            if (contact.distance < 0f)
            {
                var obiCollider = GetColliderBasedOnContact(contact);
                var ball = obiCollider.GetComponent<BallOld>();
                if (obiCollider != null)
                {
                    _currentActor = GetCurrentActor(solver, contact);

                    _currentBallrb = obiCollider.GetComponent<Rigidbody>();
                    ApplyForceToCollider(contact, _currentActor, solver);
                    CallContactPointEvent(solver, contact, ball);
                }

                break;
            }
        }
    }

    private void CallContactPointEvent(ObiSolver solver, Oni.Contact contact, BallOld ballOld)
    {
        // Chuyen tu world space sang local space
        Vector3 pointBWorld = contact.pointB;
        Vector3 pointBUnity = solver.transform.TransformPoint(pointBWorld);

        _contactPointEvent.RaiseEvent(ballOld.Point, pointBUnity);
        AudioManager.Instance.PlaySound(_contactPointClip);
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

    private void ApplyForceToCollider(Oni.Contact contact, ObiSolver.ParticleInActor actor, ObiSolver solver)
    {
        _inDirForce = _currentBallrb.velocity;

        if (actor != null && actor.actor.TryGetComponent<RopeResizing>(out var ropeResizing))
        {
            _normal = ropeResizing.GetNormalRope();
            // Helper.Vector.VectorsSameDirection(_inDirForce, ref _normal);
            _reflectForce = Vector3.Reflect(_inDirForce, _normal);

            #region Draw Line

            Vector3 pointBWorld = contact.pointB;
            Vector3 pointBUnity = solver.transform.TransformPoint(pointBWorld);

            _contactPoint = pointBUnity;

            #endregion
        }

        ApplyForceCondition(_reflectForce);
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

    private void OnDrawGizmos()
    {
        Debug.DrawLine(_contactPoint, _contactPoint + _inDirForce, Color.red);
        Debug.DrawLine(_contactPoint, _contactPoint + _normal, Color.blue);
        Debug.DrawLine(_contactPoint, _contactPoint + _reflectForce, Color.green);
    }
}