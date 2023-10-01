using System;
using UnityEngine;

namespace Apolos.Core
{
    public class CollisionContactPoint : MonoBehaviour
    {
        [SerializeField] private float _forceApplyToBall;

        private void OnCollisionEnter(Collision other)
        {
            var ball = other.gameObject.GetComponent<Ball>();

            if (ball != null)
            {
                ApplyForceToBall(other, ball);
            }
        }

        private void ApplyForceToBall(Collision other, Ball ball)
        {
            var directionVector3 = CalculatorDirection(other);
            ball.AddForce(_forceApplyToBall * -directionVector3);
        }

        private Vector3 CalculatorDirection(Collision other)
        {
            var directionCollision = (transform.position - other.transform.position).normalized;
            print(directionCollision);
            var directionVector3 = new Vector3(directionCollision.x, directionCollision.y, 0);
            return directionVector3;
        }
    }
}

