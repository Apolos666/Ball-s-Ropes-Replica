using DG.Tweening;
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
            transform.DOMove(currentWorldPos + _dragOffset, _snapTime);
            _onDrag?.Invoke();
        }
        
        private Vector3 ScreenToWorldPoint(Vector3 pos)
        {
            return GameManager.Instance.MainCamera.ScreenToWorldPoint(pos);
        }
    }
}


