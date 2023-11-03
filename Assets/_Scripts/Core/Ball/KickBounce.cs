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
    // [SerializeField] private float _dotProductImpulseThreshold = 0.8f;
    // [SerializeField] private float _timePreventMutiCollider = 0.2f;
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

    // private Quaternion _bounceBonusDir = Quaternion.Euler(0, 0, 60f);
    // private Quaternion _bounceBonusDirSameDirection = Quaternion.Euler(0, 0, 120f);
    // private Quaternion _bounceBonusDirDiffDirection = Quaternion.Euler(0, 0, -120f);
    // private Quaternion _bounceBonusDirLowAngle = Quaternion.Euler(0, 0, -60f);

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
            if (collision.gameObject.CompareTag("Can Gained Point"))
            {
                _physicRopes = collision.gameObject.GetComponentInParent<PhysicRopes>();
                ContactPoint contact = collision.contacts[0];

                var reflectVelocity = Helper.GetReflectProjectile(_lastFramedVelocity, contact.normal);

                var reflectVelocityLimit = new Vector3(
                    Mathf.Clamp(reflectVelocity.x, -_maxVelocityX, _maxVelocityX),
                    Mathf.Clamp(reflectVelocity.y, -_maxVelocityY, _maxVelocityY)
                );;

                if (reflectVelocityLimit.normalized.y < 0 && contact.normal.y > 0)
                {
                    reflectVelocityLimit.y = -reflectVelocityLimit.y * _closeRopeKickStrength;
                }
                
                _onPointCollider.RaiseEvent(_point, contact.point);
                
                AudioManager.Instance.PlaySound(_clip);
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
        // yield return new WaitForSeconds(_timePreventMutiCollider);
        yield return null;
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
