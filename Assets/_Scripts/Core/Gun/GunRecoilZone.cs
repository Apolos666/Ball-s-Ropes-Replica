using System;
using System.Collections;
using UnityEngine;

public class GunRecoilZone : MonoBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private float _gunForce;
    private Vector3 _dirEnterTrigger;

    private IEnumerator OnTriggerEnter(Collider collider)
    {
        _dirEnterTrigger = (collider.transform.position - transform.position).normalized;
        var force = _dirEnterTrigger * _gunForce * Time.fixedDeltaTime;
        var angle = Mathf.Atan2(_dirEnterTrigger.x, _dirEnterTrigger.y) * Mathf.Rad2Deg;
        
        if (collider.gameObject.TryGetComponent(out Rigidbody rb))
        {
            if (collider.gameObject.TryGetComponent(out CustomGravity gravity))
            {
                gravity.SetGravityScale(1f);
            }
            else
            {
                yield return null;
            }
            
            rb.AddForce(force, ForceMode.Impulse);
            _gun.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _dirEnterTrigger);
    }
}
