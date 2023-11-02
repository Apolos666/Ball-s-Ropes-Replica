using System;
using System.Collections;
using Apolos.Core;
using Apolos.SO;
using Apolos.System;
using UnityEngine;
using UnityEngine.Serialization;

public class KickBounce : MonoBehaviour
{
    [FormerlySerializedAs("_onBallCollider")] [SerializeField] private PointEventChannelSO _onPointCollider;
    [SerializeField] private AudioClip _clip;
    
    [SerializeField] private float _kickStrength = 1.0f;
    [SerializeField] private float _closeRopeKickStrength = 10f;
    [SerializeField] private float _maxVelocityX = 4.68f;
    [SerializeField] private float _maxVelocityY = 9.12f;
    [SerializeField] private float _dotProductImpulseThreshold = 0.8f;
    [SerializeField] private float _timePreventMutiCollider = 0.2f;
    [SerializeField] private float _point = 5f;
    private Ball _ball;
    
    private Rigidbody _rigidbody;
    private PhysicRopes _physicRopes;
    
    private bool hasCollided = false;
    private bool _isInPipe = true;

    private Vector3 _lastFramedVelocity;
    private Vector3 _contactPoint;
    private Vector3 _forceLimit;
    private Vector3 _relativeVelocity;
    private Vector3 _reflectVelocity;

    private Quaternion _bounceBonusDir = Quaternion.Euler(0, 0, 60f);
    private Quaternion _bounceBonusDirSameDirection = Quaternion.Euler(0, 0, 120f);
    private Quaternion _bounceBonusDirDiffDirection = Quaternion.Euler(0, 0, -120f);
    private Quaternion _bounceBonusDirLowAngle = Quaternion.Euler(0, 0, -60f);

    [SerializeField] private float _maxY;
    [SerializeField] private float _maxX;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ball = GetComponent<Ball>();
    }

    private void Start()
    {
        _ball.OnBallPassPipe += () => _isInPipe = false;
        _ball.OnBallReturnPipe += () => _isInPipe = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Rope") || collision.gameObject.CompareTag("Gun"))
            {
                _physicRopes = collision.gameObject.GetComponentInParent<PhysicRopes>();
                ContactPoint contact = collision.contacts[0];

                // _contactPoint = contact.point;
                
                // Tam thoi comment
                // Vector3 impulseForce;
                // Vector3 relativeVelocity;

                // if (contact.point.y > 0)
                // {
                //     impulseForce = _bounceBonusDir * collision.impulse;
                // }
                // else
                // {
                //     impulseForce = _bounceBonusDir * new Vector3(collision.impulse.x, -collision.impulse.y, collision.impulse.z);
                // }
                
                // Tam thoi comment
                // if (contact.normal.y > 0)
                // {
                //     relativeVelocity = _bounceBonusDir * collision.relativeVelocity;
                // }
                // else
                // {
                //     relativeVelocity = _bounceBonusDirLowAngle * collision.relativeVelocity;
                // }

                // var forceDir = impulseForce - contact.point;
                
                // Tam thoi comment
                // var forceDir = relativeVelocity - contact.point;
                //
                // var force = (forceDir * _kickStrength);

                // print(forceLimit);

                // Tam thoi comment
                // if (_physicRopes != null)
                // {
                //     var dot = Vector3.Dot(force.normalized, _physicRopes.GetRopeDirectionNormalized());
                //     
                //     if (DetermineQuadrant(force.normalized, dot) == 3 ||
                //         DetermineQuadrant(force.normalized, dot) == 4 && contact.point.y > 0)
                //     {
                //         force = -force;
                //     }
                //
                //     print(dot);
                //
                //     if (dot >= _dotProductImpulseThreshold)
                //     {
                //         force = _bounceBonusDirSameDirection * force * _kickStrength;
                //     }
                //     else if (dot <= -_dotProductImpulseThreshold)
                //     {
                //         force = _bounceBonusDirDiffDirection * force * _kickStrength;
                //     }
                // }
                //
                // var forceLimit = new Vector3
                // (
                //     Mathf.Clamp(force.x, -_maxVelocityX, _maxVelocityX),
                //     Mathf.Clamp(force.y, -_maxVelocityY, _maxVelocityY)
                //     // force.y
                // );
                
                // print(forceLimit);
                
                // Tam thoi comment
                // _forceLimit = forceLimit;
                // _relativeVelocity = collision.relativeVelocity;
                //
                // _rigidbody.AddForce(forceLimit * Time.fixedDeltaTime, ForceMode.Impulse);

                var reflectVelocity = Helper.GetReflectProjectile(_lastFramedVelocity, contact.normal);

                var reflectVelocityLimit = new Vector3(
                    Mathf.Clamp(reflectVelocity.x, -_maxVelocityX, _maxVelocityX),
                    Mathf.Clamp(reflectVelocity.y, -_maxVelocityY, _maxVelocityY)
                );;

                // var temp = reflectVelocityLimit;

                // var angleReflectAndRope = Vector3.Angle(reflectVelocityLimit.normalized,
                //     _physicRopes.GetRopeDirectionNormalized());;
                //
                // // var startTransform = transform.position;
                // // var predictedPosition = startTransform + (_reflectVelocity * _kickStrength) * Time.fixedDeltaTime;
                // //
                // // // print(Vector3.Distance(startTransform, predictedPosition));
                //
                // var reflectTooCloseRope = angleReflectAndRope is <= 180f and >= 160f;

                if (reflectVelocityLimit.normalized.y < 0 && contact.normal.y > 0)
                {
                    reflectVelocityLimit.y = -reflectVelocityLimit.y * _closeRopeKickStrength;
                }
                
                // if (reflectTooCloseRope && contact.normal.y > 0)
                // { 
                //     reflectVelocityLimit.y *= 3;
                // }

                // if (!reflectTooCloseRope)
                // {
                    _onPointCollider.RaiseEvent(_point, contact.point);
                
                    AudioManager.Instance.PlaySound(_clip);
                // }

                // if (contact.normal.y < 0)
                // {
                //     reflectVelocityLimit = temp;
                // }
                
                _contactPoint = contact.point;
                _reflectVelocity = reflectVelocity;
                
                
                
                _rigidbody.velocity = reflectVelocityLimit * _kickStrength * Time.fixedDeltaTime;
            }
            
            hasCollided = true;
            
            StartCoroutine(ResetCollisionFlag());
        }
    }

    private int DetermineQuadrant(Vector3 movingVector, float dot)
    {
        int value;
        
        if (dot >= 0)
        {
            value = movingVector.y >= 0 ? 1 : 4;
        }
        else
        {
            value = movingVector.y >= 0 ? 2 : 3;
        }
        
        return value;
    }

    private IEnumerator ResetCollisionFlag()
    {
        yield return new WaitForSeconds(_timePreventMutiCollider); 
        hasCollided = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_contactPoint, _contactPoint + _reflectVelocity);
        
        Vector3 position = transform.position; // Thay đổi thành vị trí của game object của bạn
        Vector3 dir = Vector3.right; // Chúng ta muốn vẽ arc theo hướng Vector3.right
        float anglesRange = 60f; // Góc 60 độ
        float radius = 1.0f; // Đặt bán kính của arc, thay đổi nếu cần
        int maxSteps = 20; // Số bước tối đa, thay đổi nếu cần

        Helper.DrawWireArc(position, dir, anglesRange, radius, maxSteps);
        Helper.DrawWireArc(position, -dir, anglesRange, radius, maxSteps);
        
    }

    private void Update()
    {
        _lastFramedVelocity = _rigidbody.velocity;
        
        if (_rigidbody.velocity.x > _maxX)
        {
            _maxX = _rigidbody.velocity.x;
        }
        
        if (_rigidbody.velocity.y > _maxY)
        {
            _maxY = _rigidbody.velocity.y;
        }
    }

    private void FixedUpdate()
    {
        if (!_isInPipe)
        {
            _rigidbody.velocity = new Vector3(
                Mathf.Clamp(_rigidbody.velocity.x, -_maxVelocityX, _maxVelocityX),
                Mathf.Clamp(_rigidbody.velocity.y, -_maxVelocityY, _maxVelocityY)
            );
        }
        else
        {
            return;
        }
    }
}
