using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _loadScene;
    
    public void SwitchScene()
    {
        var currentLevel = PlayerPrefs.GetString("Level_number");
        
        print(currentLevel);
        
        LoadingScreenHelper.Instance.CallUnLoadAsyncSceneCoroutine(currentLevel);
        LoadingScreenHelper.Instance.CallLoadAsyncSceneCoroutine(_loadScene);
        
        GenerateGrid.ClearDictionaryGlobal();
    }
}
