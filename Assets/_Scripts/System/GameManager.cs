using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Apolos.System;
using Apolos.System.EventManager;
using UnityEngine;

public class GameManager : Singleton<GameManager>, ISetUpGameObject
{
    private Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    [SerializeField] private List<GameObject> _setUpGOs = new List<GameObject>();
    [SerializeField] private string _testMapNumber;
    
    public override void Awake()
    {
        base.Awake();
        SetUpGameObjects();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.SetString("Level_number", $"Level {_testMapNumber}");
        }
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
        SetUpEvents();
    }

    private void SetUpEvents()
    {
        EventManager.AddListener("LevelCompleted", SavePlayerData);
    }

    private void SavePlayerData()
    {
        bool success = int.TryParse(LoadingScreenHelper.Instance.GetCurrentLevelNumber(), out int currentLevel);

        if (success)
        {
            PlayerPrefs.SetString("Level_number", $"Level {currentLevel + 1}");
        }
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
