using System;
using System.Collections;
using Apolos.SO;
using Apolos.System;
using UnityEngine;
using UnityEngine.Pool;

namespace Apolos.Core
{
    public class BallOld : MonoBehaviour
    {
        public Action OnBallPassPipe;
        public Action OnBallReturnPipe;
        
        private Rigidbody _rigidbody;
        private ObjectPool<BallOld> _pool;
        [HideInInspector] public bool IsRelease;
        [SerializeField] private float _point;
        public float Point => _point;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private PhysicMaterial _ballOutSidePipe;
        [SerializeField] private float _maxVelocityX = 4f;
        [SerializeField] private float _maxVelocityY = 8f;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void SetPool(ObjectPool<BallOld> pool)
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

        private void Update()
        {
            _rigidbody.velocity = new Vector3(
                Mathf.Clamp(_rigidbody.velocity.x, -_maxVelocityX, _maxVelocityX),
                Mathf.Clamp(_rigidbody.velocity.y, -_maxVelocityY, _maxVelocityY),
                0f);
        }
    }
}


