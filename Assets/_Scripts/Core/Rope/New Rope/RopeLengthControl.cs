using Obi;
using UnityEngine;

public class RopeLengthControl : MonoBehaviour
{
    [SerializeField] private ObiRopeCursor _cursor;
    [SerializeField] private ObiRope _rope;
    [SerializeField] private Transform _startRope;
    [SerializeField] private Transform _endRope;
    [SerializeField] private float changeValue = 0.5f;

    private void Update()
    {
        print("rest length" + _rope.restLength);
    }

    public void OnDrag()
    {
        var segmentDistance = Vector3.Distance(_startRope.position, _endRope.position);
        var lackingDistance = segmentDistance - _rope.restLength;
        var lackingDistanceAbs = Mathf.Abs(lackingDistance);
        
        _cursor.ChangeLength(_rope.restLength + lackingDistance - changeValue);
    }
}