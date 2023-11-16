using System;
using Apolos.SO;
using Apolos.System.EventManager;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GeneralSpawner : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _onNewItem;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _offset = new Vector3(-1, 0, 0);

    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", SetGameObject);
    }

    private void SetGameObject()
    {
        gameObject.SetActive(false);
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

    [Button("Instant Gun")]
    public void InstantGun()
    {
        OnEventRaised();
    }

    private void OnEventRaised()
    {
        GameObject rope = Instantiate(_prefab, transform.position + _offset, Quaternion.identity, transform);
        rope.transform.DOMove(_target.position, 1f);
    }
}
