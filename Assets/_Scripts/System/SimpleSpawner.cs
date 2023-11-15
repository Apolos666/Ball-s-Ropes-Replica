using System;
using Unity.Mathematics;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint;

    private void Awake()
    {
        InvokeRepeating("SpawnerAfterSec", 2.0f, 2f);
    }

    private void SpawnerAfterSec()
    {
        Instantiate(_ballPrefab, transform.position, quaternion.identity, _spawnPoint);
    }
}
