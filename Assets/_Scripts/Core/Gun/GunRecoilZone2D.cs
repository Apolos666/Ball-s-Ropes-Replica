using System;
using UnityEngine;

public class GunRecoilZone2D : MonoBehaviour
{
    // Collision Ref
    private Vector2 _normalForce;
    private Vector2 _contactPoint;
    private Vector2 _bounceDir;
    private Vector2 _inDirForce;
    private Vector2 _dirEnterCollision;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Ball"))
        {
           
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(_contactPoint, _contactPoint + _inDirForce, Color.red);
        Debug.DrawLine(_contactPoint, _contactPoint + _normalForce, Color.blue);
        Debug.DrawLine(_contactPoint, _contactPoint + _bounceDir, Color.green);
    }
}
