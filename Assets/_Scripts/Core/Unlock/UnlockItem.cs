using System;
using Apolos.System;
using Apolos.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Apolos.Core
{
    public class UnlockItem : MonoBehaviour
    {
        [SerializeField] private FundManager _fundManager;
        [SerializeField] private float _initPrice;
        private float _currentPrice;
        public float CurrentPrice => _currentPrice;
        [SerializeField] private float _increasePercent;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private AudioClip _purchaseAudio;
        
        private void Awake()
        {
            _priceText.text = $"${_initPrice}K";
            _panel.SetActive(true);
            _button.enabled = false;
            _currentPrice = _initPrice;
        }

        public void Purchase()
        {
            _fundManager.CurrentMoney -= _currentPrice;
            _currentPrice *= _increasePercent;
            _priceText.text = $"${Math.Round(_currentPrice, 1, MidpointRounding.AwayFromZero)}K";
            AudioManager.Instance.PlaySound(_purchaseAudio);
        }
    }
}


