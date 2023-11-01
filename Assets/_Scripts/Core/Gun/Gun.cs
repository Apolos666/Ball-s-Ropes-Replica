using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Line renderer Configuration")]
    private const int CIRCLE_POINT_COUNT = 24;
    [SerializeField] [ReadOnly] private bool _isUsingWorldSpace = false;
    [SerializeField] private float _lineRendererWidth = 0.1f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _yOffset;
    private int _rotateStep;
    private Vector3[] _linePoint;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        Init();
        GenerateCircle();
    }

    private void Init()
    {
        Setup();
        Configure();
    }

    private void Setup()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Configure()
    {
        _lineRenderer.positionCount = CIRCLE_POINT_COUNT + 1;
        _lineRenderer.useWorldSpace = _isUsingWorldSpace;
        _lineRenderer.widthMultiplier = _lineRendererWidth;
        _rotateStep = 360 / CIRCLE_POINT_COUNT;
        _linePoint = new Vector3[CIRCLE_POINT_COUNT];
    }

    [Button("Regenerate Circle")]
    private void GenerateCircle()
    {
        for (int i = 0; i < CIRCLE_POINT_COUNT; i++)
        {
            var x = _radius * Mathf.Cos(i * _rotateStep * Mathf.Deg2Rad);

            var y = _radius * Mathf.Sin(i * _rotateStep * Mathf.Deg2Rad);

            var pos = new Vector3(x, y - _yOffset, 0);

            _linePoint[i] = pos;
        }

        _lineRenderer.SetPositions(_linePoint);
        _lineRenderer.SetPosition(CIRCLE_POINT_COUNT, _linePoint[0]);
    }
}
