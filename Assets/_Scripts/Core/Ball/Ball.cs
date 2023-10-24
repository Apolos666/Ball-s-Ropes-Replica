using System;
using System.Collections;
using System.Collections.Generic;
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

        private bool _isCollding = false;

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
            if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
            {
                _pool.Release(this);
                this.GetComponent<TrailRenderer>().enabled = false;
                IsRelease = true;
            } else if (other.gameObject.layer == LayerMask.NameToLayer("Pipe"))
            {
                gameObject.layer = LayerMask.NameToLayer("Ball");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (this._isCollding) return;
            this._isCollding = true;
            
            if (collision.gameObject.CompareTag("Rope"))
            {
                var contactPoint = collision.GetContact(0).point;
                
                _onBallCollider.RaiseEvent(Point, contactPoint);
                
                AudioManager.Instance.PlaySound(_clip);
            }
            
            StartCoroutine(ResetTime());
        }

        private IEnumerator ResetTime()
        {
            yield return new WaitForEndOfFrame();
            this._isCollding = false;
        }
    }
}


