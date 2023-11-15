using System;
using UnityEngine;

public class SetUpCameraToCanvas : MonoBehaviour
{
    private Canvas _canvas;
    
    private void Awake()
    {
        gameObject.SetActive(true);
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = GameManager.Instance.MainCamera;
    }
}
