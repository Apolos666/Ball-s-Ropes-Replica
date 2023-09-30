using System;
using TMPro;
using UnityEngine;

namespace Apolos.Core
{
    [RequireComponent(typeof(LineRenderer))]
    public class Ropes : MonoBehaviour
    {
        private const int ROPES_COUNT = 2;
    
        private LineRenderer _lineRenderer;
        private GameObject[] _ropesSegment = new GameObject[ROPES_COUNT];

        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private Material _ropeTooLongMat;
        [SerializeField] private Material _ropeDefaultMat;
        [SerializeField] private float _ropeDistanceAllow;
        [SerializeField] private TextMeshPro _warningText;

        private void Awake()
        {
            SetUpLineRenderer();
            InitRopesSegment();
        }

        private void Update()
        {
            if (DistanceRopesSegment() > _ropeDistanceAllow)
            {
                RopeTooLong();
            }
            else
            {
                DefaultRope();
            }
            
            UpdateRopesSegment();
        }

        private void RopeTooLong()
        {
            _lineRenderer.material = _ropeTooLongMat;
            _warningText.enabled = true;
            _warningText.rectTransform.right = DirectionRopesSegment();
            _warningText.rectTransform.localPosition = MidPointRopesSegment();
        }

        private void DefaultRope()
        {
            _warningText.enabled = false;
            _lineRenderer.material = _ropeDefaultMat;
        }
    
        private void SetUpLineRenderer()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = ROPES_COUNT;
            _lineRenderer.widthCurve = _animationCurve;
        }

        private void InitRopesSegment()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Draggable>())
                    _ropesSegment[i] = transform.GetChild(i).gameObject;
            }
        }

        private void UpdateRopesSegment()
        {
            for (int i = 0; i < _ropesSegment.Length; i++)
            {
                _lineRenderer.SetPosition(i, _ropesSegment[i].transform.position);
            }
        }

        private Vector3 MidPointRopesSegment()
        {
            return (_ropesSegment[0].transform.localPosition + _ropesSegment[1].transform.localPosition) / 2f;
        }

        private float DistanceRopesSegment()
        {
            return Vector3.Distance(_ropesSegment[0].transform.position, _ropesSegment[1].transform.position);
        }

        private Vector3 DirectionRopesSegment()
        {
            return (_ropesSegment[1].transform.position - _ropesSegment[0].transform.position).normalized;
        }
    }
}


