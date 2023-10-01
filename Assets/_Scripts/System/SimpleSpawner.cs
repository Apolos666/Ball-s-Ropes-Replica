using System;
using Unity.Mathematics;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(_ballPrefab, transform.position, quaternion.identity);
        }
    }
}
