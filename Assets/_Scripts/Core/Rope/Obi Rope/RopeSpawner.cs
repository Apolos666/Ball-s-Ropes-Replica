using Apolos.SO;
using Apolos.System.EventManager;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _onNewItem;
    [SerializeField] private Material _defaultMateral;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _offset = new Vector3(-1, 0, 0);

    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", SetGameObject);
    }

    private void SetGameObject()
    {
        // gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("LevelCompleted", SetGameObject);
    }

    private void OnEnable()
    {
        _onNewItem.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        _onNewItem.OnEventRaised -= OnEventRaised;
    }

    [Button("Instant Rope")]
    public void InstantRope()
    {
        OnEventRaised();
    }

    private void OnEventRaised()
    {
        GameObject rope = Instantiate(_prefab, transform.position + _offset, Quaternion.identity, _target.transform);

        if (rope.TryGetComponent<RopeRefContainer>(out var ropeRefContainer))
        {
            ropeRefContainer.RopeController2D.OnCreateRope(_defaultMateral);
        }

        rope.transform.DOMove(_target.position, 1f);
    }
}
