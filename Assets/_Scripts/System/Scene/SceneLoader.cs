using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _loadScene;
    [SerializeField] private string _unLoadScene;
    
    public void SwitchScene()
    {
        LoadingScreenHelper.Instance.CallUnLoadAsyncSceneCoroutine(_unLoadScene);
        LoadingScreenHelper.Instance.CallLoadAsyncSceneCoroutine(_loadScene);
    }
}
