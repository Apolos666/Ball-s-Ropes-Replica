using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Apolos.Core
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private ObjectPool<Ball> _pool;
        public bool IsRelease;

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
    }
}


