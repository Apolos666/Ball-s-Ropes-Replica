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
            if (!SceneManager.GetSceneByName("Main Menu").IsValid())
            {
                LoadRequirementScene();
                break;
            }
        }
    }

    private void LoadRequirementScene()
    {
        StartCoroutine(LoadingScreenHelper.Instance.LoadAsyncScene("Main Menu")); 

        _sceneLoadedTask.SetResult(true);
    }
}
