using System;
using Apolos.System.EventManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAnyPosScreen : MonoBehaviour, IPointerDownHandler
{
    private bool _isLoadingScene;
    private string _currentLevel;

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentLevel = PlayerPrefs.GetString("Level_number");
        
        if (_isLoadingScene) return;
        
        LoadingScreenHelper.Instance.CallUnLoadAsyncSceneCoroutine("Main Menu");

        if (string.IsNullOrEmpty(_currentLevel))
        {
            LoadingScreenHelper.Instance.CallLoadAsyncSceneCoroutine("Level 1");
            PlayerPrefs.SetString("Level_number", "Level 1");
        }
        else
        {
            if (LoadingScreenHelper.Instance.IsSceneValid(_currentLevel))
            {
                LoadingScreenHelper.Instance.CallLoadAsyncSceneCoroutine(_currentLevel);
            }
            else
            {
                Debug.LogError("Scene khong nam trong build setting");
            }
        }
    }

    private void Awake()
    {
        EventManager.AddListener("LoadingScene", () => _isLoadingScene = true);
        EventManager.AddListener("LoadingSceneComplete", () => _isLoadingScene = false);
        EventManager.AddListener("LoadingToMainMenu", () => gameObject.SetActive(true));
        EventManager.AddListener("UnloadingMainMenu", () => gameObject.SetActive(false));
    }
}
