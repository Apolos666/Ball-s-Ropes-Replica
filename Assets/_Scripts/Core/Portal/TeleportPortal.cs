using System;
using Apolos.Core;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnPoint, _endSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        
        var ball = other.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            var ballRB = ball.GetComponent<Rigidbody>();
            ball.transform.position = _endSpawnPoint.position;
            ballRB.velocity = new Vector3(-ballRB.velocity.x, ballRB.velocity.y);
        }
    }
}
