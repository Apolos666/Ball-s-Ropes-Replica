using System;
using Apolos.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Apolos.Core
{
    public class AffordMoneyChecker : MonoBehaviour
    {
        [SerializeField] private FundManager _fundManager;
        [SerializeField] private UnlockItem _unlockItem;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;

        private bool _canAfford;
        public bool CanAfford => _canAfford;
        
        private void Start()
        {
            _fundManager.OnChangedCurrentMoney += OnChangedCurrentMoney;
        }

        private void OnDestroy()
        {
            _fundManager.OnChangedCurrentMoney -= OnChangedCurrentMoney;
        }

        private void OnChangedCurrentMoney(float currentMoney)
        {
            if (_unlockItem.CurrentPrice > currentMoney)
            {
                _canAfford = false;
                _button.enabled = false;
                _panel.SetActive(true);
            }
            else
            {
                _canAfford = true;
                _button.enabled = true;
                _panel.SetActive(false);
            }
        }
    }
}


