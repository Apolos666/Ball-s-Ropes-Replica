using System;
using Apolos.System.EventManager;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", SetGameObjectState);
    }

    private void SetGameObjectState()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        EventManager.RemoveListener("LevelCompleted", SetGameObjectState);
    }
}
