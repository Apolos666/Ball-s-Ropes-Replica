using System;
using Apolos.Core;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnPoint, _endSpawnPoint;
    [SerializeField] private float _forceApplyX = 120f;

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            var ballRB = ball.GetComponent<Rigidbody>();
            var ballTrail = ball.GetComponent<TrailRenderer>();
            ball.transform.position = _endSpawnPoint.position;
            ballRB.velocity = Vector3.zero;
            ballTrail.Clear();
            ballRB.velocity = new Vector3(_forceApplyX * Time.fixedDeltaTime, ballRB.velocity.y);
        }
    }
}
