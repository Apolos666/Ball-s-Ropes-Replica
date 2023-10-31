using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CustomUIComponent : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    protected abstract void Setup();
    protected abstract void Configure();
    
    [Button("Reconfigure")]
    private void Init()
    {
        Setup();
        Configure();
    }

    private void OnValidate()
    {
        Init();
    }
}
