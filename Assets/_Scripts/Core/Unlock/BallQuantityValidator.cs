using System;
using Apolos.SO;
using Apolos.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Apolos.Core
{
    public class BallQuantityValidator : MonoBehaviour
    {
        [SerializeField] private NormalBallSpawner _normalBallSpawner;
        [SerializeField] private VoidEventChannelSO _onChangedBallsPerWave;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _panel;

        private void Start()
        {
            _onChangedBallsPerWave.OnEventRaised += QuantityValidation;
        }

        private void OnDestroy()
        {
            _onChangedBallsPerWave.OnEventRaised -= QuantityValidation;
        }

        public void QuantityValidation()
        {
            if (_normalBallSpawner.IsUpgrable())
            {
                print("Can Upgrade");
                _button.enabled = true;
                _panel.SetActive(false);
            }
            else
            {
                print("Can not upgrade");
                _button.enabled = false;
                _panel.SetActive(true);
            }
        }
    }
}


