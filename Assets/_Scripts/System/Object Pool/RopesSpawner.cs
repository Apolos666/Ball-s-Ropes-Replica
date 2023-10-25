using System;
using Apolos.SO;
using Apolos.System.EventManager;
using DG.Tweening;
using UnityEngine;

public class RopesSpawner : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _onNewRopes;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _ropePrefab;
    [SerializeField] private Vector3 _offset = new Vector3(-1, 0, 0);

    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", () => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        _onNewRopes.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        _onNewRopes.OnEventRaised -= OnEventRaised;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnEventRaised();
        }
    }

    private void OnEventRaised()
    {
        GameObject rope = Instantiate(_ropePrefab, transform.position + _offset, Quaternion.identity, transform);
        rope.transform.DOMove(_target.position + _offset, 1f);
    }
}
