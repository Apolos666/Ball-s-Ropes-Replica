using System;
using Apolos.Core;
using UnityEngine;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnPoint, _endSpawnPoint;

    private void OnCollisionEnter(Collision other)
    {
        var ball = other.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            ball.transform.position = _endSpawnPoint.position;
        }
    }
}
