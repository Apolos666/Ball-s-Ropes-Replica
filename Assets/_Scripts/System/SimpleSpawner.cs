using System;
using Unity.Mathematics;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;

    private void Awake()
    {
        // InvokeRepeating("SpawnerAfterSec", 2.0f, 5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(_ballPrefab, transform.position, quaternion.identity);
        }
    }

    private void SpawnerAfterSec()
    {
        Instantiate(_ballPrefab, transform.position, quaternion.identity);
    }
}
