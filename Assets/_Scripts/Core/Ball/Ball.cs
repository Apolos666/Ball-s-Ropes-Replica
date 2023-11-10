using System;
using System.Collections;
using Apolos.SO;
using Apolos.System;
using UnityEngine;
using UnityEngine.Pool;

namespace Apolos.Core
{
    public class Ball : MonoBehaviour
    {
        public Action OnBallPassPipe;
        public Action OnBallReturnPipe;
        
        private Rigidbody _rigidbody;
        private ObjectPool<Ball> _pool;
        [HideInInspector] public bool IsRelease;
        [SerializeField] private float _point;
        public float Point => _point;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private PhysicMaterial _ballOutSidePipe;
        
        private bool _isCollding = false;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
                OnBallReturnPipe?.Invoke();
            } else if (other.gameObject.layer == LayerMask.NameToLayer("Pipe"))
            {
                gameObject.GetComponent<Collider>().sharedMaterial = _ballOutSidePipe;
                gameObject.layer = LayerMask.NameToLayer("Ball");
                OnBallPassPipe?.Invoke();
            }
        }
    }
}


