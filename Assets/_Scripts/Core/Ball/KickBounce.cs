using System;
using System.Collections;
using UnityEngine;

public class KickBounce : MonoBehaviour
{
    [SerializeField] private float _kickStrength = 1.0f;
    [SerializeField] private float _maxVelocityX = 4.68f;
    [SerializeField] private float _maxVelocityY = 9.12f;
    [SerializeField] private float _dotProductImpulseThreshold = 0.8f;
    [SerializeField] private float _timePreventMutiCollider = 0.2f;
    
    private Rigidbody _rigidbody;
    private PhysicRopes _physicRopes;
    
    private bool hasCollided = false;

    private Vector3 _contactPoint;
    private Vector3 _forceLimit;
    private Vector3 _relativeVelocity;

    private Quaternion _bounceBonusDir = Quaternion.Euler(0, 0, 60f);
    private Quaternion _bounceBonusDirSameDirection = Quaternion.Euler(0, 0, 120f);
    private Quaternion _bounceBonusDirDiffDirection = Quaternion.Euler(0, 0, -120f);
    private Quaternion _bounceBonusDirLowAngle = Quaternion.Euler(0, 0, -60f);

    [SerializeField] private float _maxY;
    [SerializeField] private float _maxX;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Rope"))
            {
                _physicRopes = collision.gameObject.GetComponentInParent<PhysicRopes>();
                ContactPoint contact = collision.contacts[0];

                _contactPoint = contact.point;

                Vector3 impulseForce;
                Vector3 relativeVelocity;

                // if (contact.point.y > 0)
                // {
                //     impulseForce = _bounceBonusDir * collision.impulse;
                // }
                // else
                // {
                //     impulseForce = _bounceBonusDir * new Vector3(collision.impulse.x, -collision.impulse.y, collision.impulse.z);
                // }
                
                if (contact.normal.y > 0)
                {
                    relativeVelocity = _bounceBonusDir * collision.relativeVelocity;
                }
                else
                {
                    relativeVelocity = _bounceBonusDirLowAngle * collision.relativeVelocity;
                }

                // var forceDir = impulseForce - contact.point;
                
                var forceDir = relativeVelocity - contact.point;
                
                var force = (forceDir * _kickStrength);

                // print(forceLimit);

                if (_physicRopes != null)
                {
                    var dot = Vector3.Dot(force.normalized, _physicRopes.GetRopeDirectionNormalized());
                    
                    if (DetermineQuadrant(force.normalized, dot) == 3 ||
                        DetermineQuadrant(force.normalized, dot) == 4 && contact.point.y > 0)
                    {
                        force = -force;
                    }
                
                    print(dot);

                    if (dot >= _dotProductImpulseThreshold)
                    {
                        force = _bounceBonusDirSameDirection * force * _kickStrength;
                    }
                    else if (dot <= -_dotProductImpulseThreshold)
                    {
                        force = _bounceBonusDirDiffDirection * force * _kickStrength;
                    }
                }
                
                var forceLimit = new Vector3
                (
                    Mathf.Clamp(force.x, -_maxVelocityX, _maxVelocityX),
                    Mathf.Clamp(force.y, -_maxVelocityY, _maxVelocityY)
                    // force.y
                );
                
                // print(forceLimit);

                _forceLimit = forceLimit;
                _relativeVelocity = collision.relativeVelocity;

                _rigidbody.AddForce(forceLimit * Time.fixedDeltaTime, ForceMode.Impulse);
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
        yield return new WaitForSeconds(_timePreventMutiCollider); // Đặt lại biến cờ sau 0.1 giây
        hasCollided = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_contactPoint, _contactPoint + _relativeVelocity.normalized);
        
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
        if (_rigidbody.velocity.x > _maxX)
        {
            _maxX = _rigidbody.velocity.x;
        }
        
        if (_rigidbody.velocity.y > _maxY)
        {
            _maxY = _rigidbody.velocity.y;
        }
    }
    
    


    
}
