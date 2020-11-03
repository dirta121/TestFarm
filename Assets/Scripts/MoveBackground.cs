using UnityEngine;
using UnityEngine.EventSystems;
namespace TestFarm
{
    public class MoveBackground : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 _lastMousePosition;
        private RectTransform _rect;
        private void Start()
        {
            _rect = GetComponent<RectTransform>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Begin Drag");
            _lastMousePosition = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - _lastMousePosition;
            Vector3 newPosition = _rect.position + new Vector3(diff.x, diff.y, transform.position.z);
            Vector3 oldPos = _rect.position;
            newPosition = new Vector3(newPosition.x, oldPos.y, newPosition.z);
            _rect.position = newPosition;
            if (_rect.anchoredPosition.x <= -_rect.rect.width / 2 + Screen.width / 2) _rect.anchoredPosition = new Vector2(-_rect.rect.width / 2 + Screen.width / 2, _rect.anchoredPosition.y);
            if (_rect.anchoredPosition.x >= +_rect.rect.width / 2 - Screen.width / 2) _rect.anchoredPosition = new Vector2(+_rect.rect.width / 2 - Screen.width / 2, _rect.anchoredPosition.y);

            _lastMousePosition = currentMousePosition;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End Drag");
        }
    }
}