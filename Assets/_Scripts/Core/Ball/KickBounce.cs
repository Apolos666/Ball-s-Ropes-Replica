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
    private BallOld _ballOld;
    
    private Rigidbody _rigidbody;
    private PhysicRopes _physicRopes;
    
    private bool hasCollided = false;
    private bool _isInPipe = true;

    private Vector3 _lastFramedVelocity;
    private Vector3 _contactPoint;
    private Vector3 _forceLimit;
    private Vector3 _relativeVelocity;
    private Vector3 _reflectVelocity;

    [SerializeField] private float _maxY;
    [SerializeField] private float _maxX;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ballOld = GetComponent<BallOld>();
    }

    private void Start()
    {
        _ballOld.OnBallPassPipe += () => _isInPipe = false;
        _ballOld.OnBallReturnPipe += () => _isInPipe = true;
    }
    
    private IEnumerator ResetCollisionFlag()
    {
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
