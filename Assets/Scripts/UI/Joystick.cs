using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private float _handleRange = 1f;
    [SerializeField] private float _deadZone = 0.2f;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _camera;

    private Vector2 _inputVector = Vector2.zero;

    public Vector2 Direction => _inputVector;
    public float Horizontal => _inputVector.x;
    public float Vertical => _inputVector.y;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_background == null) 
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _background,
            eventData.position,
            _camera,
            out Vector2 position
        );

        position /= _background.sizeDelta * _canvas.scaleFactor;
        _inputVector = position.magnitude > _deadZone ? position : Vector2.zero;
        _inputVector = _inputVector.magnitude > 1f ? _inputVector.normalized : _inputVector;
        _handle.anchoredPosition = _inputVector * (_background.sizeDelta.x * 0.5f * _handleRange);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}