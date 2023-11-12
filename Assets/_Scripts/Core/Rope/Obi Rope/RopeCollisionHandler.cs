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
            if (contact.distance < -0f)
            {
                var obiCollider = GetColliderBasedOnContact(contact);
                var ball = obiCollider.GetComponent<Ball>();
                if (obiCollider != null)
                {
                    _currentActor = GetCurrentActor(solver, contact);

                    _currentBallrb = obiCollider.GetComponent<Rigidbody>();
                    ApplyForceToCollider(contact, _currentActor, solver);
                    CallContactPointEvent(solver, contact, ball);
                }
            }
        }
    }

    private void CallContactPointEvent(ObiSolver solver, Oni.Contact contact, Ball ball)
    {
        // Chuyen tu world space sang local space
        Vector3 pointBWorld = contact.pointB;
        Vector3 pointBUnity = solver.transform.TransformPoint(pointBWorld);

        _contactPointEvent.RaiseEvent(ball.Point, pointBUnity);
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
        var inDir = _currentBallrb.velocity;

        Vector3 reflectForce = Vector3.zero;

        if (actor != null && actor.actor.TryGetComponent<RopeResizing>(out var ropeResizing))
        {
            var normal = ropeResizing.GetNormalRope();
            reflectForce = Vector3.Reflect(inDir, normal);
            // Chuyen tu world space sang local space
            Vector3 pointBWorld = contact.pointB;
            Vector3 pointBUnity = solver.transform.TransformPoint(pointBWorld);
            Debug.DrawLine(pointBUnity, pointBUnity + inDir, Color.red);
            Debug.DrawLine(pointBUnity, pointBUnity + normal, Color.blue);
            Debug.DrawLine(pointBUnity, pointBUnity + reflectForce, Color.green);
        }

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