using System;
using Apolos.System.EventManager;
using Apolos.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicRopes : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private PhysicMaterial _ropePhysicMaterial;
    [SerializeField] private PhysicMaterial _startEndMaterial;
    [SerializeField] private Material _ropeDefaultMaterial;
    [SerializeField] private Material _ropeTooLongMaterial;
    
    [SerializeField] private Transform _startRope, _endRope;
    [SerializeField] private int _segmentCount = 20;
    [SerializeField] private float _totalLength = 5f;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _colliderRadius = 0.13f;
    [SerializeField] private int _sides = 10;
    [SerializeField] private float _linearLimitSpringDamping;
    [SerializeField] private float _linearLimitSpring;
    [SerializeField] private float _linearLimitBounce;

    [SerializeField] private bool _usePhysics = true;

    [SerializeField] private bool _useGravity = false;
    [SerializeField] private float _totalWeight = 5f;
    [SerializeField] private float _drag = 1;
    [SerializeField] private float _angularDrag = 1f;
    [SerializeField] private RigidbodyInterpolation _ropeInterpolation;
    [SerializeField] private CollisionDetectionMode _ropeDetectionMode;
    [SerializeField] private RigidbodyConstraints _ropeConstraints;

    private Transform[] _segments = Array.Empty<Transform>();
    [SerializeField] private Transform _segmentParent;
    private int _previousSegmentCount;
    private float _prevTotalLength;
    private float _prevDrag;
    private float _prevTotalWeight;
    private float _prevAngularDrag;
    private double _prevRadius;

    private MeshDataRopes _meshData;
    
    private Vector3[] _vertices;
    private int[,] _vertexIndicesMap;
    private bool _createTriangles;
    private Mesh _mesh;
    private int _prevSides;

    private float _ropeDistance;
    [SerializeField] private float _ropeLengthTolerance = 1f;
    [SerializeField] private float _ropeDistanceAllow = 3f;
    [SerializeField] private float _ropeDistanceShort = 1.2f;
    private bool _isAlreadyRopeDefault = true;
    private bool _isAlreadyRopeTooLong = false;
    private bool _isAlreadyRopeTooShort = false;

    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", SetGameObjectState);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("LevelCompleted", SetGameObjectState);
    }

    private void SetGameObjectState()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _vertices = new Vector3[_segmentCount * _sides * 3];
        GenerateMesh();
    }

    public Vector3 GetRopeDirectionNormalized()
    {
        return (_endRope.position - _startRope.position).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_startRope.position, _startRope.position + GetRopeDirectionNormalized());
    }

    private void Update()
    {
        if (_previousSegmentCount != _segmentCount)
        {
            RemoveSegments();
            _segments = new Transform[_segmentCount];
            GenerateSegments();
            GenerateMesh();
        }
        
        _previousSegmentCount = _segmentCount;
        
        if (_totalLength != _prevTotalLength || _prevDrag != _drag || _prevTotalWeight != _totalWeight ||
            _prevAngularDrag != _angularDrag)
        {
            UpdateWire();
            GenerateMesh();
        }

        if (_sides != _prevSides)
        {
            _vertices = new Vector3[_segmentCount * _sides * 3];
            GenerateMesh();
        }
        
        _prevSides = _sides;

        _prevTotalLength = _totalLength;
        _prevDrag = _drag;
        _prevTotalWeight = _totalWeight;
        _prevAngularDrag = _angularDrag;

        if (_prevRadius != _radius && _usePhysics)
        {
            UpdateRadius();
            GenerateMesh();
        }
        
        _prevRadius = _radius;
        
        UpdateMesh();
    }

    public void OnDrag()
    {
        _ropeDistance = Vector3.Distance(_startRope.position, _endRope.position) - _ropeLengthTolerance;
        
        _totalLength = _ropeDistance;

        if (Vector3.Distance(_startRope.position, _endRope.position) > _ropeDistanceAllow)
        {
            if (_isAlreadyRopeTooLong) return;
            AdjustRope(false, true, false, 50, _ropeTooLongMaterial);
        }
        else if (Vector3.Distance(_startRope.position, _endRope.position) > _ropeDistanceShort)
        {
            if (_isAlreadyRopeDefault) return;
            AdjustRope(true, false, false, 20, _ropeDefaultMaterial);
        }
        else
        {
            if (_isAlreadyRopeTooShort) return;
            AdjustRope(false, false, true, 10, _ropeDefaultMaterial);
        }
    }

    private void AdjustRope(bool isAlreadyRopeDefault, bool isAlreadyRopeTooLong, bool isAlreadyRopeTooShort, int segmentCount, Material ropeMaterial)
    {
        _isAlreadyRopeDefault = isAlreadyRopeDefault;
        _isAlreadyRopeTooLong = isAlreadyRopeTooLong;
        _isAlreadyRopeTooShort = isAlreadyRopeTooShort;

        _segmentCount = segmentCount;
        // SetSegmentCollider(true);
        _meshRenderer.sharedMaterial = ropeMaterial;
    }

    private void UpdateMesh()
    {
        GenerateVertices();
        _mesh.RecalculateNormals();
        _meshFilter.mesh.vertices = _vertices;
    }

    private void GenerateMesh()
    {
        _createTriangles = true;

        if (_meshData == null)
        {
            _meshData = new MeshDataRopes(_sides, _segmentCount + 1, false);
        }
        else
        {
            _meshData.ResetMesh(_sides, _segmentCount + 1, false);
        }
        
        GenerateIndicesMap();
        GenerateVertices();
        _meshData.ProcessMesh();
        _mesh = _meshData.CreateMesh();

        _meshFilter.sharedMesh = _mesh;
        
        _createTriangles = false;
    }

    private void GenerateIndicesMap()
    {
        _vertexIndicesMap = new int[_segmentCount + 1, _sides + 1];
        int meshVertexIndex = 0;
        for (int segmentIndex = 0; segmentIndex < _segmentCount; segmentIndex++)
        {
            for (int sideIndex = 0; sideIndex < _sides; sideIndex++)
            {
                _vertexIndicesMap[segmentIndex, sideIndex] = meshVertexIndex;
                meshVertexIndex++;
            }
        }
    }
    
    private void GenerateVertices()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            GenerateCircleVerticesAndTriangles(_segments[i], i);
        }
    }

    private void GenerateCircleVerticesAndTriangles(Transform segmentTransform, int segmentIndex)
    {
        float angleDiff = 360f / _sides;

        Quaternion diffRotation = Quaternion.FromToRotation(Vector3.forward, segmentTransform.forward);

        for (int sideIndex = 0; sideIndex < _sides; sideIndex++)
        {
            float angleInRad = sideIndex * angleDiff * Mathf.Deg2Rad;
            float x = -1 * _radius * Mathf.Cos(angleInRad);
            float y = _radius * Mathf.Sin(angleInRad);

            Vector3 pointOffset = new(x, y, 0);

            Vector3 pointRotated = diffRotation * pointOffset;

            Vector3 pointRotatedAtCenterOfTransform = segmentTransform.localPosition + pointRotated;

            int vertexIndex = segmentIndex * _sides + sideIndex;
            _vertices[vertexIndex] = pointRotatedAtCenterOfTransform;

            if (_createTriangles)
            {
                _meshData.AddVertex(pointRotatedAtCenterOfTransform, new Vector2(0, 0), vertexIndex);

                bool createThisTriangle = segmentIndex < _segmentCount - 1;
                if (createThisTriangle)
                {
                    int currentIncrement = 1;
                    int a = _vertexIndicesMap[segmentIndex, sideIndex];
                    int b = _vertexIndicesMap[segmentIndex + currentIncrement, sideIndex];
                    int c = _vertexIndicesMap[segmentIndex, sideIndex + currentIncrement];
                    int d = _vertexIndicesMap[segmentIndex + currentIncrement, sideIndex + currentIncrement];

                    bool isLastGap = sideIndex == _sides - 1;
                    if (isLastGap)
                    {
                        c = _vertexIndicesMap[segmentIndex, 0];
                        d = _vertexIndicesMap[segmentIndex + currentIncrement, 0];
                    }
                    
                    _meshData.AddTriangle(a, d, c);
                    _meshData.AddTriangle(d, a, b);
                }
            }
        }
    }

    private void UpdateRadius()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            SetRadiusOnSegment(_segments[i], _radius);
        }
    }

    private void SetRadiusOnSegment(Transform segment, float radius)
    {
        segment.TryGetComponent<SphereCollider>(out SphereCollider sphereCollider);
        sphereCollider.radius = radius;
    }

    private void UpdateWire()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            if (i != 0)
            {
                UpdateLengthOnSegment(_segments[i], _totalLength / _segmentCount);
            }

            UpdateWeightOnSegment(_segments[i], _totalWeight, _drag, _angularDrag);
        }
    }

    private void UpdateWeightOnSegment(Transform segment, float totalWeight, float drag, float angularDrag)
    {
        transform.TryGetComponent<Rigidbody>(out Rigidbody rigidbody);

        if (rigidbody != null)
        {
            rigidbody.mass = _totalWeight / _segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }
    }

    private void UpdateLengthOnSegment(Transform segment, float segmentCount)
    {
        segment.TryGetComponent<ConfigurableJoint>(out ConfigurableJoint joint);
        if (joint != null)
        {;
            joint.connectedAnchor = Vector3.forward * segmentCount;
        }
    }

    private void RemoveSegments()
    {
        if (_segments == null) return;
        
        for (int i = 0; i < _segments.Length; i++)
        {
            if (_segments[i] != null)
            {
                Destroy(_segments[i].gameObject);
            }
        }
    }
    
    private void GenerateSegments()
    {
        JointSegment(_startRope, null, _ropePhysicMaterial,true, false, false);
        Transform prevSegment = _startRope;

        Vector3 direction = _endRope.position - _startRope.position;

        for (int i = 0; i < _segmentCount; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}");
            segment.transform.SetParent(_segmentParent);
            segment.layer = LayerMask.NameToLayer("Segment");
            segment.tag = "Can Gained Point";

            Vector3 pos = prevSegment.position + (direction / _segmentCount);
            segment.transform.position = pos;

            if (i == 0 || i == _segmentCount - 1)
            {
                JointSegment(segment.transform, prevSegment, _startEndMaterial);            }
            else
            {
                JointSegment(segment.transform, prevSegment, _ropePhysicMaterial);
            }

            _segments[i] = segment.transform;
            prevSegment = segment.transform;
        }
        
        JointSegment(_endRope, prevSegment, _ropePhysicMaterial,true, true, false);
    }

    private void JointSegment(Transform currentTransform, Transform connectedTransform, PhysicMaterial physicMaterial, bool isKinematic = false, bool isCloseConnected = false, bool usePhysics = true)
    {
        if (currentTransform.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rigidbody = currentTransform.AddComponent<Rigidbody>();
            rigidbody.useGravity = _useGravity;
            rigidbody.isKinematic = isKinematic;
            rigidbody.mass = _totalWeight / _segmentCount;
            rigidbody.drag = _drag;
            rigidbody.angularDrag = _angularDrag;
            rigidbody.interpolation = _ropeInterpolation;
            rigidbody.collisionDetectionMode = _ropeDetectionMode;
            rigidbody.constraints = _ropeConstraints;
        }
        
        if (usePhysics)
        {
            SphereCollider sphereCollider = currentTransform.AddComponent<SphereCollider>();
            if (_isAlreadyRopeDefault)
            {
                sphereCollider.enabled = true;
            }
            else if (_isAlreadyRopeTooLong)
            {
                sphereCollider.enabled = false;
            }

            sphereCollider.material = physicMaterial;
            sphereCollider.radius = _colliderRadius;
        }

        if (connectedTransform != null)
        {
            ConfigurableJoint configurableJoint = currentTransform.GetComponent<ConfigurableJoint>();

            if (configurableJoint == null)
            {
                configurableJoint = currentTransform.AddComponent<ConfigurableJoint>();
            }

            configurableJoint.connectedBody = connectedTransform.GetComponent<Rigidbody>();

            configurableJoint.autoConfigureConnectedAnchor = false;

            if (isCloseConnected)
            {
                configurableJoint.anchor = Vector3.zero;
                configurableJoint.connectedAnchor = Vector3.forward * 0.1f;
            }
            else
                configurableJoint.connectedAnchor = Vector3.forward * _totalLength / _segmentCount;

            configurableJoint.xMotion = ConfigurableJointMotion.Limited;
            configurableJoint.yMotion = ConfigurableJointMotion.Limited;
            configurableJoint.zMotion = ConfigurableJointMotion.Limited;

            configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit
            {
                limit = 0f, bounciness = _linearLimitBounce
            };

            SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring
            {
                spring = _linearLimitSpring, damper = _linearLimitSpringDamping
            };

            configurableJoint.linearLimit = softJointLimit;

            configurableJoint.linearLimitSpring = softJointLimitSpring;
            
            configurableJoint.angularZLimit = softJointLimit;

            JointDrive jointDrive = new JointDrive
            {
                positionSpring = 0,
                positionDamper = 0
            };
            configurableJoint.angularXDrive = jointDrive;
            configurableJoint.angularYZDrive = jointDrive;
        }

    }
}
