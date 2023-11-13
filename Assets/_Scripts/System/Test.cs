using System;
using System.Collections;
using System.Collections.Generic;
using Apolos.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class Test : MonoBehaviour
{
    private SplineAnimate _splineAnimate;

    private void Awake()
    {
        _splineAnimate = GetComponent<SplineAnimate>();
        
    }

    [Button("Change Value")]
    private void ChangeValue()
    {
        _splineAnimate.NormalizedTime = 0.5f;
    }
}
