using System;
using Apolos.SO;
using Apolos.System;
using UnityEngine;
using UnityEngine.Pool;

namespace Apolos.Core
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ObjectPool<Ball> _pool;
        [HideInInspector] public bool IsRelease;
        public float Point;
        [SerializeField] private BallEventChannelSO _onBallCollider;
        [SerializeField] private AudioClip _clip;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void AddForce(Vector3 forceApply)
        {
            _rigidbody.AddForce(forceApply, ForceMode.Impulse);
        }

        public void SetPool(ObjectPool<Ball> pool)
        {
            _pool = pool;
        }

        private void OnTriggerEnter(Collider other)
        {
            _pool.Release(this);
            IsRelease = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Rope"))
            {
                var contactPoint = collision.GetContact(0).point;
                
                _onBallCollider.RaiseEvent(Point, contactPoint);
                
                AudioManager.Instance.PlaySound(_clip);
            }
        }
    }
}


