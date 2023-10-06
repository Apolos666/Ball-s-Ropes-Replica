using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickEffect : MonoBehaviour
{
    private Vector3 _originalScale;
    
    [SerializeField] private Vector3 _doEaseInOutElastic;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _button;
    public Action OnCompleteAnimation;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void DoEaseInOutElsatic()
    {
        transform.DOScale(_doEaseInOutElastic, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            transform.DOScale(_originalScale, 0.5f);
            _panel.SetActive(false);
            _button.enabled = true;
            OnCompleteAnimation?.Invoke();
        });
    }
}
