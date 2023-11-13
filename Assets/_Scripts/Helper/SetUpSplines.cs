using System;
using UnityEngine;
using UnityEngine.Splines;

public class SetUpSplines : MonoBehaviour
{
    [SerializeField] private float _startOffset;
    private SplineAnimate _splineAnimate;

    private void Awake()
    {
        SetUp();
        Config();
    }

    private void Config()
    {
        _splineAnimate.StartOffset = _startOffset;
    }

    private void SetUp()
    {
        _splineAnimate = GetComponent<SplineAnimate>();
    }
}
