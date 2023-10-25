using System.Collections.Generic;
using Apolos.System;
using Apolos.System.EventManager;
using UnityEngine;

public class GameManager : Singleton<GameManager>, ISetUpGameObject
{
    private Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    [SerializeField] private List<GameObject> _setUpGOs = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        SetUpGameObjects();
    }

    private void SetUpGameObjects()
    {
        foreach (var setUpGO in _setUpGOs)
        {
            var setUpGOInterface = setUpGO.GetComponent<ISetUpGameObject>();

            if (setUpGOInterface != null)
            {
                setUpGOInterface.SetUpGameObject();
            }
        }
    }
    
    public void SetUpGameObject()
    {
        SetUpGameManager();
    }
    
    private void SetUpGameManager()
    {
        _mainCamera = Camera.main;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        EventManager.RaiseEvent("OnPauseGame");
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        EventManager.RaiseEvent("OnContinueGame");
    }
}
