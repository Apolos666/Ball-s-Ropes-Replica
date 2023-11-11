using Obi;
using UnityEngine;

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
            if (contact.distance < -0f)
            {
                var obiCollider = GetColliderBasedOnContact(contact);
                if (obiCollider != null)
                {
                    _currentActor = GetCurrentActor(solver, contact);

                    _currentBallrb = obiCollider.GetComponent<Rigidbody>();
                    ApplyForceToCollider(contact, _currentActor);
                }
            }
        }
    }

    private static ObiSolver.ParticleInActor GetCurrentActor(ObiSolver solver, Oni.Contact contact)
    {
        int particleIndex = solver.simplices[contact.bodyA];

        ObiSolver.ParticleInActor pa = solver.particleToActor[particleIndex];
        return pa;
    }

    private void ApplyForceToCollider(Oni.Contact contact, ObiSolver.ParticleInActor actor)
    {
        var inDir = _currentBallrb.velocity;

        Vector3 reflectForce = Vector3.zero;

        if (actor != null && actor.actor.TryGetComponent<RopeResizing>(out var ropeResizing))
        {
            print(actor.actor.GetInstanceID());
            var normal = ropeResizing.GetNormalRope();
            reflectForce = Vector3.Reflect(inDir, normal);
            Debug.DrawLine(contact.pointB, (Vector3)contact.pointB + inDir, Color.red);
            Debug.DrawLine(contact.pointB, (Vector3)contact.pointB + normal, Color.blue);
            Debug.DrawLine(contact.pointB, (Vector3)contact.pointB + reflectForce, Color.green);
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