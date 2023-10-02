using System;
using UnityEngine;

namespace Apolos.Core
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;   

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void AddForce(Vector3 forceApply)
        {
            _rigidbody.AddForce(forceApply, ForceMode.Impulse);
        }
    }
}


