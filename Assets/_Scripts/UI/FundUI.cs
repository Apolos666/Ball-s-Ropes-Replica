using Apolos.UI;
using TMPro;
using UnityEngine;

namespace System.UI
{
    public class FundUI : MonoBehaviour
    {
        [SerializeField] private FundManager _fundManager;
        [SerializeField] private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            _textMesh.text = "0K";
        }

        private void OnEnable()
        {
            _fundManager.OnChangedCurrentMoney += OnChangedCurrentMoney;
        }

        private void OnDisable()
        {
            _fundManager.OnChangedCurrentMoney -= OnChangedCurrentMoney;
        }

        private void OnChangedCurrentMoney(float value)
        {
            _textMesh.text = $"{Math.Round(value, 1, MidpointRounding.AwayFromZero)}K";
        }
    }
}


