using System;
using Apolos.System.EventManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAnyPosScreen : MonoBehaviour, IPointerDownHandler
{
    private bool _isLoadingScene;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isLoadingScene) return;
        
        StartCoroutine(LoadingScreenHelper.Instance.UnloadAsyncScene("Main Menu"));
        StartCoroutine(LoadingScreenHelper.Instance.LoadAsyncScene("Game Scene"));
    }

    private void Awake()
    {
        EventManager.AddListener("LoadingScene", () => _isLoadingScene = true);
        EventManager.AddListener("LoadingSceneComplete", () => _isLoadingScene = false);
        EventManager.AddListener("LoadingToMainMenu", () => gameObject.SetActive(true));
        EventManager.AddListener("UnloadingMainMenu", () => gameObject.SetActive(false));
    }
}
