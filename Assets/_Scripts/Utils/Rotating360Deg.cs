using System;
using DG.Tweening;
using UnityEngine;

public class Rotating360Deg : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateValue;
    [SerializeField] private RotateMode _rotateMode = RotateMode.FastBeyond360;
    [SerializeField] private Ease _easeMode = Ease.Linear;
    [SerializeField] private float _duration = 2f;
    
    private void Awake()
    {
        transform.DORotate(_rotateValue, _duration, _rotateMode).SetRelative(true).SetLoops(-1).SetEase(_easeMode);
    }
}
