using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static TaskCompletionSource<bool> _sceneLoadedTask = new TaskCompletionSource<bool>();

    private void Start()
    {
        // load all scenes
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (!SceneManager.GetSceneByName("Game Scene").IsValid())
            {
                StartCoroutine(LoadRequirementScene());
                break;
            }
        }
    }

    private IEnumerator LoadRequirementScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("Game Scene", LoadSceneMode.Additive);
        
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;
            
            yield return null;
        }

        _sceneLoadedTask.SetResult(true);
    }
}
