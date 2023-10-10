using System;
using Apolos.SO;
using UnityEngine;
using UnityEngine.UI;

public class MergeBallBtn : MonoBehaviour
{
    [SerializeField] private ButtonClickEffect _buttonClickEffect;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _button;

    private int _normalBallsPerWave;

    private void Awake()
    {
        _panel.SetActive(true);
        _button.enabled = false;
    }

    private void Start()
    {
        _buttonClickEffect.OnCompleteAnimation += OnCompleteAnimation;
    }

    private void OnDestroy()
    {
        _buttonClickEffect.OnCompleteAnimation -= OnCompleteAnimation;
    }
    
    private void OnCompleteAnimation()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_normalBallsPerWave > 3)
        {
            _panel.SetActive(false);
            _button.enabled = true;
        }
        else
        {
            _panel.SetActive(true);
            _button.enabled = false;
        }
    }

    private void Update()
    {
        UpdateUI();
    }
}
