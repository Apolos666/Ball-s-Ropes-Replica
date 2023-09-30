using Apolos.System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Camera _mainCamera;
    public Camera MainCamera => _mainCamera; 

    public override void Awake()
    {
        base.Awake();
        SetUpGameManager();
    }

    private void SetUpGameManager()
    {
        _mainCamera = Camera.main;
    }
}
