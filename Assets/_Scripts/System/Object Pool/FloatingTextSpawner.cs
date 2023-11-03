using System;
using Apolos.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Apolos.System
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        private ObjectPool<FloatingText> _pool;
        [SerializeField] private FloatingText _floatingTextPrefab;
        [SerializeField] private int _defaultCapacity = 200;
        [SerializeField] private int _maxSize = 300;
        [FormerlySerializedAs("_onPointCollider")] [FormerlySerializedAs("_onBallCollider")] 
        [SerializeField] private PointEventChannelSO _onGetPoint;

        private Vector3 _contactPoint;
        private string _money;

        private void OnEnable()
        {
            _onGetPoint.OnEventRaised += OnEventRaised;
        }

        private void OnDisable()
        {
            _onGetPoint.OnEventRaised -= OnEventRaised;
        }

        private void OnEventRaised(float value, Vector3 contactPoint)
        {
            _contactPoint = contactPoint;
            _money = $"{value.ToString()}$";
            _pool.Get();
        }

        private void Awake()
        {
            _pool = new ObjectPool<FloatingText>(CreateFloatingText, OnTakeFloatingTextFromPool, OnReleaseToPool,
                OnDestroyFloatingText, true, _defaultCapacity, _maxSize);
        }

        private void OnDestroyFloatingText(FloatingText floatingText)
        {
            Destroy(floatingText.gameObject);
        }

        private void OnReleaseToPool(FloatingText floatingText)
        {
            floatingText.gameObject.SetActive(false);
        }

        private void OnTakeFloatingTextFromPool(FloatingText floatingText)
        {
            floatingText.GetComponent<TextMeshPro>().text = _money;
            floatingText.transform.position = _contactPoint;
            floatingText.transform.localScale = Vector3.one;

            floatingText.gameObject.SetActive(true);
            
            floatingText.FloatingTextAnimation();
        }

        private FloatingText CreateFloatingText()
        {
            var floatingText = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity, transform);
            floatingText.gameObject.SetActive(false);
            floatingText.SetPool(_pool);
            return floatingText;
        }
    }    
}


