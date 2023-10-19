using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopesCollider : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void SetCollider(bool isEnable, GameObject segment)
    {
        SphereCollider segmentCollider = segment.GetComponent<SphereCollider>();
        segmentCollider.enabled = isEnable;
    }
}