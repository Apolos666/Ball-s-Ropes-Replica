using System;
using Apolos.System.EventManager;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
}
