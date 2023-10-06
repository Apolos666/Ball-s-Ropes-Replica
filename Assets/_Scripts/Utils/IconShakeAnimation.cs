using System;
using DG.Tweening;
using UnityEngine;

public class IconShakeAnimation : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _desiredScale;
    
    private void Awake()
    {
        transform.DOScale(transform.localScale + _desiredScale, _duration).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutElastic);
    }
}
