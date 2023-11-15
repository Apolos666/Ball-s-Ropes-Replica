using System;
using System.Collections;
using Apolos.Core;
using Apolos.SO;
using Apolos.System;
using UnityEngine;

public class GunRecoilZone : MonoBehaviour
{
    [SerializeField] private PointEventChannelSO _onPointCollider;
    [SerializeField] private AudioClip _gunClip;
    public Action OnGunCollision;
    [SerializeField] private Transform _gun;
    [SerializeField] private float _gunForce;
    private Vector3 _dirEnterTrigger;

    private IEnumerator OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Ball")) yield return null;
            
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

            var ball = collider.GetComponent<BallOld>();

            _gun.transform.rotation = Quaternion.Euler(0, 0, -angle);
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
            OnGunCollision?.Invoke();
            _onPointCollider.RaiseEvent(ball.Point, collider.transform.position);
            AudioManager.Instance.PlaySound(_gunClip);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _dirEnterTrigger);
    }
}
