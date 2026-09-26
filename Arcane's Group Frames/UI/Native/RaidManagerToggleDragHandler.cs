using UnityEngine;
using UnityEngine.EventSystems;

namespace ArcanesGroupFrames
{
    internal sealed class RaidManagerToggleDragHandler :
        MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        private RectTransform _rectTransform;

        private Canvas _canvas;

        private Vector2 _dragOffset;


        private void Awake()
        {
            _rectTransform =
                GetComponent<RectTransform>();


            _canvas =
                GetComponentInParent<Canvas>();
        }


        public void OnBeginDrag(
            PointerEventData eventData)
        {
            if (_rectTransform == null)
            {
                return;
            }


            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent
                    as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 pointerLocal);


            _dragOffset =
                _rectTransform.anchoredPosition -
                pointerLocal;
        }


        public void OnDrag(
            PointerEventData eventData)
        {
            if (_rectTransform == null)
            {
                return;
            }


            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent
                    as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 pointerLocal))
            {
                return;
            }


            _rectTransform.anchoredPosition =
                pointerLocal +
                _dragOffset;


            ClampToScreen();
        }


        public void OnEndDrag(
            PointerEventData eventData)
        {
            ClampToScreen();


            RaidManagerToggleButton
                .StoreCurrentPosition();
        }


        private void ClampToScreen()
        {
            if (_rectTransform == null)
            {
                return;
            }


            RectTransform parent =
                _rectTransform.parent
                    as RectTransform;


            if (parent == null)
            {
                return;
            }


            Vector2 position =
                _rectTransform.anchoredPosition;


            float halfWidth =
                _rectTransform.rect.width *
                0.5f;

            float halfHeight =
                _rectTransform.rect.height *
                0.5f;


            Rect parentRect =
                parent.rect;


            position.x =
                Mathf.Clamp(
                    position.x,
                    parentRect.xMin + halfWidth,
                    parentRect.xMax - halfWidth);


            position.y =
                Mathf.Clamp(
                    position.y,
                    parentRect.yMin + halfHeight,
                    parentRect.yMax - halfHeight);


            _rectTransform.anchoredPosition =
                position;
        }
    }
}