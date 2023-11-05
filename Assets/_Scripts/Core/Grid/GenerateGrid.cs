using System;
using Apolos.System.EventManager;
using Sirenix.OdinInspector;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private GameObject _attractorPrefab;
    [SerializeField] private int _gridCellSizeX = 10;
    [SerializeField] private int _gridCellSizeY = 10;
    [SerializeField] private float _cellOffset = 0.5f;
    private BoxCollider _boxCollider;
    private Vector3 _topLeftPoint;
    private Vector3 _topRightPoint;
    private Vector3 _bottomLeftPoint;
    private Vector3 _bottomRightPoint;
    private float _heightDistance;
    private float _widthDistance;
    private Vector3[,] _grid;
    [SerializeField] [ReadOnly] private GameObject[,] _attractorGOs;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SetUp();
        GeneratePoint();
        SetUpAfterGenerate();
    }

    private void SetUpAfterGenerate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 1.5f);
        _boxCollider.size = new Vector3(_boxCollider.size.x, _boxCollider.size.y, 5f);
    }

    [Button("Generate Grid")]
    private void GeneratePoint()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                if (_attractorGOs[x, y] != null) 
                    Destroy(_attractorGOs[x, y]);
                var cell = new Vector3((_topLeftPoint.x + (_widthDistance * y)) + _cellOffset, (_topLeftPoint.y - (_heightDistance * x)) - _cellOffset);
                var attractorGO = Instantiate(_attractorPrefab, cell, Quaternion.identity, transform);
                attractorGO.name = $"Attractor Point {x}_{y}";
                _attractorGOs[x, y] = attractorGO;
            }
        }
    }

    private void SetUp()
    {
        _boxCollider = GetComponent<BoxCollider>();
        GetCorners();
        CalculatedGrid();
        _grid = new Vector3[_gridCellSizeX, _gridCellSizeY];
        _attractorGOs = new GameObject[_gridCellSizeX, _gridCellSizeY];
    }

    private void CalculatedGrid()
    {
        var width = Mathf.Abs(_topLeftPoint.x - _topRightPoint.x);
        var height = Mathf.Abs(_topLeftPoint.y - _bottomLeftPoint.y);
        _widthDistance = width / _gridCellSizeX;
        _heightDistance = height / _gridCellSizeY;
    }

    private void GetCorners()
    {
        _topLeftPoint = new Vector3(_boxCollider.center.x - (_boxCollider.size.x / 2),
            _boxCollider.center.y + (_boxCollider.size.y / 2));
        _topRightPoint = new Vector3(_boxCollider.center.x + (_boxCollider.size.x / 2),
            _boxCollider.center.y + (_boxCollider.size.y / 2));
        _bottomLeftPoint = new Vector3(_boxCollider.center.x - (_boxCollider.size.x / 2),
            _boxCollider.center.y - (_boxCollider.size.y / 2));
        _bottomRightPoint = new Vector3(_boxCollider.center.x + (_boxCollider.size.x / 2),
            _boxCollider.center.y - (_boxCollider.size.y / 2));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_topLeftPoint, 0.2f);
        Gizmos.DrawWireSphere(_topRightPoint, 0.2f);
        Gizmos.DrawWireSphere(_bottomLeftPoint, 0.2f);
        Gizmos.DrawWireSphere(_bottomRightPoint, 0.2f);
    }
    
    private void Start()
    {
        EventManager.AddListener("LevelCompleted", SetGameObjectState);
    }

    private void SetGameObjectState()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        EventManager.RemoveListener("LevelCompleted", SetGameObjectState);
    }
}
