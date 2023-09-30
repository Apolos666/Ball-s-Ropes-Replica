using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Apolos.Core
{
    public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private Vector3 _dragOffset;
        [SerializeField] private float _snapTime = 0.5f;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            var fingerWorldPos = ScreenToWorldPoint(eventData.position);
            _dragOffset = transform.position - fingerWorldPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var currentWorldPos = ScreenToWorldPoint(eventData.position);
            transform.DOMove(currentWorldPos + _dragOffset, _snapTime);
        }
        
        private Vector3 ScreenToWorldPoint(Vector3 pos)
        {
            return GameManager.Instance.MainCamera.ScreenToWorldPoint(pos);
        }
    }
}


