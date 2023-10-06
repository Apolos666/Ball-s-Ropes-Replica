using DG.Tweening;
using UnityEngine;

public class IconZoomInOut : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _desiredScale;
    
    private void Awake()
    {
        transform.DOScale(transform.localScale + _desiredScale, 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
