using System;
using Apolos.System.EventManager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CompleteLevelUI : MonoBehaviour, ISetUpGameObject
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease _easeMode;
    [SerializeField] private RectTransform _completeLevel;
    [SerializeField] private RectTransform _nextButton;
    [SerializeField] private Image _backgroundImg;
    [SerializeField] private Color _initColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private float _initPositionX = -1000f;
    private Vector2 _finalPosition;

    public void CompleteLevel()
    {
        gameObject.SetActive(true);
        _completeLevel.DOLocalMove(GetFinalPosition(_completeLevel), _duration).SetEase(_easeMode);
        _nextButton.DOLocalMove(GetFinalPosition(_nextButton), _duration).SetEase(_easeMode);
        _backgroundImg.DOColor(_endColor, _duration);
    }

    private Vector2 GetFinalPosition(RectTransform rectTransform)
    {
        return new Vector2(0, rectTransform.anchoredPosition.y);
    }

    private void SetInitState()
    {
        gameObject.SetActive(false);
        _backgroundImg.color = _initColor;
        InitPosition(_completeLevel);
        InitPosition(_nextButton);
    }
    
    private void InitPosition(RectTransform rectTransform)
    {
        rectTransform.localPosition = new Vector2( _initPositionX, rectTransform.anchoredPosition.y);
    }

    public void SetUpGameObject()
    {
        gameObject.SetActive(false);
        SetInitState();
        SetUpEvent();
    }

    private void SetUpEvent()
    {
        EventManager.AddListener("LevelCompleted", CompleteLevel);
    }
}
