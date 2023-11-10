using System;
using Obi;
using UnityEngine;

public class RopeResizing : MonoBehaviour
{
    [SerializeField] private ObiRope _obiRope;
    [SerializeField] private ObiRopeCursor _obiRopeCursor;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;

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

        _obiRopeCursor.ChangeLength(_obiRope.CalculateLength() - 0.2f);
    }

    public void RopeTooLong()
    {
        // Thay metarial va disable collider
    }
    
    public void OnCompleteChange()
    {
        int mask = (1 << 15) ;
        var myFilter = ObiUtils.MakeFilter(mask, 1);
        foreach (var t in _obiRope.solverIndices)
            _obiRope.solver.filters[t] = myFilter;
    }
}
