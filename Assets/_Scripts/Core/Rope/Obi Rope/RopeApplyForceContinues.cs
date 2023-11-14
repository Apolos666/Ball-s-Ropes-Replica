using System;
using Obi;
using UnityEngine;

public class RopeApplyForceContinues : MonoBehaviour
{
    [SerializeField] private Transform _startRope, _endRope;
    private ObiSolver _obiSolver;
    [SerializeField] private float _forceApply;

    private void Awake()
    {
        _obiSolver = GetComponent<ObiSolver>();
    }

    private void Start()
    {
        _obiSolver.OnCollision += OnCollision;
    }

    private void OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        foreach (var contact in e.contacts)
        {
            if (contact.distance < 0)
            {
                var obiCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;

                if (obiCollider.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.AddForce(Helper.Vector.GetNormalVector(_startRope, _endRope) * _forceApply, ForceMode2D.Impulse);
                }

                break;
            }
        }
        
    }
}
