using System;
using Apolos.System.EventManager;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener("LevelCompleted", () => gameObject.SetActive(false));
    }
}
