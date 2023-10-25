using System;
using System.Collections;
using System.Text.RegularExpressions;
using Apolos.System;
using Apolos.System.EventManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenHelper : Singleton<LoadingScreenHelper>
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadingBar;

    public override void Awake()
    {
        base.Awake();
        _loadingBar.fillAmount = 0f;
    }

    public void CallLoadAsyncSceneCoroutine(string sceneName)
    {
        _loadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    public void CallUnLoadAsyncSceneCoroutine(string sceneName)
    {
        _loadingScreen.SetActive(true);
        StartCoroutine(UnloadAsyncScene(sceneName));
    }

    private IEnumerator LoadAsyncScene(string sceneName)
    {
        EventManager.RaiseEvent("LoadingScene");
        
        _loadingBar.fillAmount = 0f;
        
        yield return new WaitForSeconds(0.1f);
        
        Application.backgroundLoadingPriority = ThreadPriority.High;
        
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        _loadingBar.fillAmount = asyncLoad.progress;
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            var progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            _loadingBar.fillAmount = progress;

            if (Mathf.Approximately(asyncLoad.progress, 0.9f))
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
 
        yield return new WaitForEndOfFrame();
        
        _loadingScreen.SetActive(false);
        _loadingBar.fillAmount = 0f;
        
        EventManager.RaiseEvent("LoadingSceneComplete");

        if (sceneName == "Main Menu")
        {
            EventManager.RaiseEvent("LoadingToMainMenu");
        }
        else
        {
            EventManager.RaiseEvent("UnloadingMainMenu");
        }
    }

    private IEnumerator UnloadAsyncScene(string sceneName)
    {
        var asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

    public string GetCurrentLevelNumber()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(sceneName);

        string levelNumber = match.Value;
        
        return levelNumber;
    }

    public bool IsSceneValid(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (String.Compare(name, sceneName, StringComparison.OrdinalIgnoreCase) == 0)
                return true;
        }

        return false;
    }
}
