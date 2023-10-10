using System;
using Apolos.SO;
using Apolos.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Apolos.Core
{
    public class BallAndMoneyValidator : MonoBehaviour
    {
        [SerializeField] private FundManager _fundManager;
        [SerializeField] private UnlockItem _unlockItem;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;
        [SerializeField] private NormalBallSpawner _normalBallSpawner;
        [SerializeField] private VoidEventChannelSO _onChangedBallsPerWave;

        private bool _canAffordMoney;
        private bool _canAffordBalls;
        
        private void Start()
        {
            _fundManager.OnChangedCurrentMoney += OnChangedCurrentMoney;
            _onChangedBallsPerWave.OnEventRaised += QuantityValidation;
        }

        private void OnDestroy()
        {
            _fundManager.OnChangedCurrentMoney -= OnChangedCurrentMoney;
            _onChangedBallsPerWave.OnEventRaised -= QuantityValidation;
        }

        private void OnChangedCurrentMoney(float currentMoney)
        {
            if (_unlockItem.CurrentPrice > currentMoney)
            {
                _canAffordMoney = false;
            }
            else
            {
                _canAffordMoney = true;
            }
        }

        private void QuantityValidation()
        {
            if (_normalBallSpawner.IsUpgrable())
            {
                _canAffordBalls = true;
            }
            else
            {
                _canAffordBalls = false;
            }
        }

        private void Update()
        {
            if (_canAffordBalls && _canAffordMoney)
            {
                _button.enabled = true;
                _panel.SetActive(false);
            }
            else
            {
                _button.enabled = false;
                _panel.SetActive(true);
            }
        }
    }
}


