using System;
using Apolos.System;
using Apolos.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apolos.Core
{
    public class UnlockItem : MonoBehaviour
    {
        [SerializeField] private FundManager _fundManager;
        [SerializeField] private float _initPrice;
        private float _currentPrice;
        [SerializeField] private float _increasePercent;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private ButtonClickEffect _buttonClickEffect;
        [SerializeField] private AudioClip _clip;

        private float _currentMoney;
        
        private void Awake()
        {
            _textMesh.text = $"${_initPrice}K";
            _panel.SetActive(true);
            _button.enabled = false;
            _currentPrice = _initPrice;
        }

        private void OnEnable()
        {
            _fundManager.OnChangedCurrentMoney += OnChangedCurrentMoney;
            _buttonClickEffect.OnCompleteAnimation += OnCompleteAnimation;
        }

        private void OnDisable()
        {
            _fundManager.OnChangedCurrentMoney -= OnChangedCurrentMoney;
            _buttonClickEffect.OnCompleteAnimation -= OnCompleteAnimation;
        }
        
        private void OnCompleteAnimation()
        {
            UpdateUI(_currentMoney);
        }

        private void OnChangedCurrentMoney(float currentMoney)
        {
            UpdateUI(currentMoney);
            _currentMoney = currentMoney;
        }

        private void UpdateUI(float currentMoney)
        {
            if (_currentPrice > currentMoney)
            {
                _panel.SetActive(true);
                _button.enabled = false;
            }
            else
            {
                _panel.SetActive(false);
                _button.enabled = true;
            }
        }

        public void Purchase()
        {
            _fundManager.CurrentMoney -= _currentPrice;
            _currentPrice *= _increasePercent;
            _textMesh.text = $"${Math.Round(_currentPrice, 1, MidpointRounding.AwayFromZero)}K";
            AudioManager.Instance.PlaySound(_clip);
        }
    }
}


