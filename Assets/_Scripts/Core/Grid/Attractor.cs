using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameElement"))
        {
            _parent.SetActive(false);
        }
    }

    public Transform GetTransformParent()
    {
        return _parent.transform;
    }
}
