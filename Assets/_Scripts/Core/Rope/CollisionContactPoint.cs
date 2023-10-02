using System;
using UnityEngine;

namespace Apolos.Core
{
    public class CollisionContactPoint : MonoBehaviour
    {
        [SerializeField] private float _maxForceApplyPointY = 7f;
        [SerializeField] private Vector3 _maxForceApplyY = new Vector3(1, 7f, 0);
        [SerializeField] private float _maxForceApplyPointX = 3f;
        [SerializeField] private Vector3 _maxForceApplyX = new Vector3(3f, 1f, 0);
        private float _forceAppliedPointY;
        private Vector3 _forceAppliedY;
        private float _forceAppliedPointX;
        private Vector3 _forceAppliedX;

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
            var x = CalculatorDirection(other).x * _forceAppliedX.x;
            var y = CalculatorDirection(other).y * _forceAppliedY.y;
            var force = new Vector3(x, y, 0);
            ball.AddForce(-force);
        }

        private Vector3 CalculatorDirection(Collision other)
        {
            var firstContactPoint = other.GetContact(0);

            var directionCollision = firstContactPoint.normal;

            CalculateAppliedForce(firstContactPoint, directionCollision);

            return directionCollision;
        }

        private void CalculateAppliedForce(ContactPoint firstContactPoint, Vector3 direction)
        {
            _forceAppliedPointY = Mathf.InverseLerp(0f, _maxForceApplyPointY,  Mathf.Abs(firstContactPoint.impulse.y));
            _forceAppliedY = Vector3.Lerp(Vector3.right, ForceApplpYBaseOnPosition(direction), _forceAppliedPointY);
            
            _forceAppliedPointX = Mathf.InverseLerp(0f, _maxForceApplyPointX,  Mathf.Abs(firstContactPoint.impulse.x));
            _forceAppliedX = Vector3.Lerp(Vector3.right,  ForceApplyXBaseOnPosition(direction), _forceAppliedPointX);
        }

        private Vector3 ForceApplyXBaseOnPosition(Vector3 direction)
        {
            return direction.y > 0 ? Quaternion.Euler( 0, 0, 75) * _maxForceApplyX * 10 : _maxForceApplyX;
        }

        private Vector3 ForceApplpYBaseOnPosition(Vector3 direction)
        {
            return direction.y > 0 ? _maxForceApplyY / 5 : _maxForceApplyY;
        }
    }
}

