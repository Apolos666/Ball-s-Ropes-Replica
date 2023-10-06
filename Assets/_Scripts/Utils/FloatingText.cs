using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour
{
    private ObjectPool<FloatingText> _pool;

    public void SetPool(ObjectPool<FloatingText> pool)
    {
        _pool = pool;
    }

    public void FloatingTextAnimation()
    {
        DOTween.Sequence()
            .Insert(0, transform.DOScale(Vector3.zero, 7).SetEase(Ease.OutQuart))
            .Insert(0, transform.DOMoveY(0.7f, 7)).SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                _pool.Release(this);
            });

    }
}
