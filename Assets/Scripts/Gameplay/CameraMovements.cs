using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _smoothSpeed;

    private Vector3 _targetPosition;

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        _targetPosition = new Vector3
        {
            x = _player.position.x,
            y = _camera.position.y,
            z = _player.position.z,
        };

        _camera.position = Vector3.Lerp(_camera.position, _targetPosition, _smoothSpeed * Time.fixedDeltaTime);
    }
}