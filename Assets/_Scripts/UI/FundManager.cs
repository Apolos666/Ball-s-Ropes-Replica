using System;
using Apolos.SO;
using UnityEngine;

namespace Apolos.UI
{
    public class FundManager : MonoBehaviour
    {
        [SerializeField] private BallEventChannelSO _onBallCollider;
        
        private float _currentMoney = 0f;
        public float CurrentMoney
        {
            get { return _currentMoney; }
            set
            {
                _currentMoney = value;
                OnChangedCurrentMoney?.Invoke(_currentMoney);
            }
        }

        private float _earnedMoney = 0f;
        public float EarnedMoney
        {
            get { return _earnedMoney; }
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
            _onBallCollider.OnEventRaised += OnBallCollider;
        }

        private void OnDisable()
        {
            _onBallCollider.OnEventRaised -= OnBallCollider;
        }

        private void OnBallCollider(float value, Vector3 vector3)
        {
            CurrentMoney += value;
            EarnedMoney += value;
        }
        
        
    }
}


