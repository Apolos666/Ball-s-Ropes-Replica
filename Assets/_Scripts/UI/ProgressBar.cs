using System;
using Apolos.System.EventManager;
using Apolos.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private float _maxAmount;
    [SerializeField] private FundManager _fundManager;
    [SerializeField] private Image _fillBar;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _fillBar.fillAmount = 0f;
        _textMesh.text = $"0K / {_maxAmount}K";
    }

    private void OnEnable()
    {
        _fundManager.OnChangedEarnedMoney += OnChangedEarnedMoney;
    }

    private void OnDisable()
    {
        _fundManager.OnChangedEarnedMoney -= OnChangedEarnedMoney;
    }

    private void OnChangedEarnedMoney(float value)
    {
        _fillBar.DOFillAmount(value / _maxAmount, 1f);
        _textMesh.text = $"{value}K / {_maxAmount}K";
        
        if (_fillBar.fillAmount >= 0.91f)
        {
            EventManager.RaiseEvent("LevelCompleted");
        }
    }
}
