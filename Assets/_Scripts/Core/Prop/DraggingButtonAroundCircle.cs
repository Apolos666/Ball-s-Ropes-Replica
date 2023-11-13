using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

public class DraggingButtonAroundCircle : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform _parent;
    [SerializeField] private SplineAnimate _splineAnimate;
    private bool _isDragging;
    private Vector3 _parentVector3Down;
    public Action<float> OnDragging;

    private void Awake()
    {
        _parentVector3Down = _parent.TransformDirection(Vector3.down);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        var dragPositionWolrd = (Vector2)GameManager.Instance.MainCamera.ScreenToWorldPoint(eventData.position);
        var currentDir = dragPositionWolrd - (Vector2)_parent.position;

        #region Draw Line

        Debug.DrawLine(_parent.position, (_parent.position + (Vector3)currentDir) * 10, Color.red);
        Debug.DrawLine(_parent.position, (_parent.position + _parentVector3Down) * 10, Color.green);

        #endregion
        var angle = Helper.Angle.CalculateVector2Angle360Deg(_parentVector3Down, currentDir);

        var convertToLerpFactor = angle / 360f;

        _splineAnimate.NormalizedTime = convertToLerpFactor;
        OnDragging?.Invoke(angle);
    }
}
