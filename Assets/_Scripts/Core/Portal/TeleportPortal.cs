using System;
using Apolos.Core;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnPoint, _endSpawnPoint;
    [SerializeField] private float _forceApplyX = 120f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.TryGetComponent<Rigidbody2D>(out var ballRB))
            {
                ballRB.velocity = Vector3.zero;
                ballRB.transform.position = _endSpawnPoint.position;
                ballRB.velocity = new Vector2(_forceApplyX * Time.fixedDeltaTime, ballRB.velocity.y);
            }
        }
    }
}
