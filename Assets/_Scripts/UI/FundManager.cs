using System;
using Apolos.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Apolos.UI
{
    public class FundManager : MonoBehaviour
    {
        [FormerlySerializedAs("_onBallCollider")] [SerializeField] private PointEventChannelSO _onPointCollider;
        
        private float _currentMoney = 0f;
        public float CurrentMoney
        {
            get => _currentMoney;
            set
            {
                _currentMoney = value;
                OnChangedCurrentMoney?.Invoke(_currentMoney);
            }
        }

        private float _earnedMoney = 0f;
        public float EarnedMoney
        {
            get => _earnedMoney;
            set
            {
                _earnedMoney = value;
                OnChangedEarnedMoney?.Invoke(_earnedMoney);
            }
        }

        public Action<float> OnChangedEarnedMoney;
        public Action<float> OnChangedCurrentMoney;

        private void OnEnable()
        {
            _onPointCollider.OnEventRaised += OnBallCollider;
        }

        private void OnDisable()
        {
            _onPointCollider.OnEventRaised -= OnBallCollider;
        }

        private void OnBallCollider(float value, Vector3 vector3)
        {
            CurrentMoney += value;
            EarnedMoney += value;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                EarnedMoney += 1000f;
            }
        }

        public void IncreaseCurrentMoney(float value)
        {
            CurrentMoney += value;
        }
        
        public void IncreaseEarnedMoney(float value)
        {
            EarnedMoney += value;
        }
    }
}


