using System;
using Obi;
using UnityEngine;

public class RopeResizing : MonoBehaviour
{
    private const float ROPE_TOO_LONG_DISTANCE = 1.7f;

    [Header("Materials")] 
    [SerializeField] private Material _normalRope;
    [SerializeField] private Material _ropeTooLong;
    [Header("Rope Configuration")]
    [SerializeField] private ObiRope _obiRope;

    private MeshRenderer _ropeMeshRenderer;
    [SerializeField] private ObiRopeCursor _obiRopeCursor;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private float _ropePrefix = 0.1f;

    private void Awake()
    {
        _ropeMeshRenderer = _obiRope.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        var distance = Vector3.Distance(_startPos.position, _endPos.position);
        
        _obiRopeCursor.ChangeLength(distance);
    }

    public void ChangeLengthRope()
    {
        var myFilter = ObiUtils.MakeFilter(ObiUtils.CollideWithNothing, 0);
        foreach (var t in _obiRope.solverIndices)
            _obiRope.solver.filters[t] = myFilter;
    } 
    
    public void OnCompleteChange()
    {
        int colliderMask = (1 << 15);
        int notCollider = ObiUtils.CollideWithNothing;

        var distance = Vector3.Distance(_startPos.position, _endPos.position);

        ChangeMaskCollider(distance >= ROPE_TOO_LONG_DISTANCE ? notCollider : colliderMask, 1);
        _ropeMeshRenderer.sharedMaterial = distance >= ROPE_TOO_LONG_DISTANCE ? _ropeTooLong : _normalRope;
    }

    public void OnCreateRope(Material material)
    {
        var myFilter = ObiUtils.MakeFilter(ObiUtils.CollideWithNothing, 0);
        foreach (var t in _obiRope.solverIndices)
            _obiRope.solver.filters[t] = myFilter;

        _ropeMeshRenderer.sharedMaterial = material;
    }

    private void ChangeMaskCollider(int maskCollider, int category)
    {
        var myFilter = ObiUtils.MakeFilter(maskCollider, category);
        foreach (var t in _obiRope.solverIndices)
            _obiRope.solver.filters[t] = myFilter;
    }

    public Vector3 GetNormalRope()
    {
        var dir = _endPos.position - _startPos.position;
        var normal = new Vector3(-dir.y, dir.x, 0);
        return normal.normalized;
    }
}
