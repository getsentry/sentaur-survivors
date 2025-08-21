using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace UI
{
    public class FloatingOnScreenStick : OnScreenControl, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _movementRange = 50;
        [SerializeField] private RectTransform _stickTransform;
        [SerializeField] private RectTransform _ringTransform;
        
        [InputControl(layout = "Vector2")]
        [SerializeField] private string _controlPath;
        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        private RectTransform _rectTransform;
        
        private Vector2 _startPos;
        private Vector2 _pointerDownPos;
        private Vector2 _dragPos;
        
        private void Start()
        {
            _rectTransform = (RectTransform)transform;
            _startPos = _stickTransform.anchoredPosition;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
            {
                return;
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _pointerDownPos);
            _stickTransform.anchoredPosition = new Vector2(_pointerDownPos.x, _rectTransform.rect.height + _pointerDownPos.y);
            _ringTransform.anchoredPosition = new Vector2(_pointerDownPos.x, _rectTransform.rect.height + _pointerDownPos.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _dragPos);
            var delta = _dragPos - _pointerDownPos;

            delta = Vector2.ClampMagnitude(delta, _movementRange);
            _stickTransform.anchoredPosition = new Vector2(_pointerDownPos.x, _rectTransform.rect.height + _pointerDownPos.y) + delta;

            var newPos = new Vector2(delta.x / _movementRange, delta.y / _movementRange);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _stickTransform.anchoredPosition = _startPos;
            _ringTransform.anchoredPosition = _startPos;
            SendValueToControl(Vector2.zero);
        }
    }
}