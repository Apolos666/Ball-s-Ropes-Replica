using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Apolos.Core
{
    public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Vector3 _dragOffset;
        [SerializeField] private float _snapTime = 0.5f;

        [SerializeField] private UnityEvent _onDrag;
        [SerializeField] private UnityEvent _onCancel;
        [SerializeField] private UnityEvent _onCompleteMove;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentTween;

        public void OnPointerDown(PointerEventData eventData)
        {
            var fingerWorldPos = ScreenToWorldPoint(eventData.position);
            _dragOffset = transform.position - fingerWorldPos;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _onCancel?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var currentWorldPos = ScreenToWorldPoint(eventData.position);
            _currentTween = transform.DOMove(currentWorldPos + _dragOffset, _snapTime);
            _onDrag?.Invoke();
        }

        public void StopMoving()
        {
            DOTween.Kill(transform);
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         StopMoving();
        //     }
        // }

        private Vector3 ScreenToWorldPoint(Vector3 pos)
        {
            return GameManager.Instance.MainCamera.ScreenToWorldPoint(pos);
        }
    }
}


