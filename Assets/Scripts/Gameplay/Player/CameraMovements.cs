using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -3);
    [SerializeField] private float _cameraVerticalRotationOffset = 1.5f;
    [SerializeField] private LayerMask _collisionLayers = -1;
    [SerializeField] private float _collisionOffset = 0.3f;

    private Vector3 _targetPosition;
    private float _currentCameraDistance;

    private void Start()
    {
        _currentCameraDistance = -_offset.z;
    }

    private void LateUpdate()
    {
        if (_player == null)
            return;

        HandleCameraCollision();
        UpdateCameraPosition();
    }

    private void HandleCameraCollision()
    {
        // ������� ������ � ������ ������ ������
        Vector3 playerEyePosition = _player.position + Vector3.up * _offset.y;

        // ����������� �� ������ � ������
        Vector3 cameraDirection = -_player.forward;

        // ������������ ��������� ������
        float maxDistance = -_offset.z;

        // ��������� ��������
        RaycastHit hit;
        if (Physics.Raycast(playerEyePosition, cameraDirection, out hit, maxDistance, _collisionLayers))
        {
            // ���� ���� ������������, ������������� ������ ����� ������������
            _currentCameraDistance = Mathf.Max(hit.distance - _collisionOffset, 0.5f);
        }
        else
        {
            // ������ ������������ � ���������� ���������
            _currentCameraDistance = Mathf.Lerp(_currentCameraDistance, maxDistance, Time.deltaTime * _smoothSpeed);
        }
    }

    private void UpdateCameraPosition()
    {
        // ������������ ������� ������ � ������ ��������
        Vector3 desiredOffset = new Vector3(0, _offset.y, -_currentCameraDistance);
        _targetPosition = _player.position + _player.forward * desiredOffset.z + Vector3.up * desiredOffset.y;

        // ������� �����������
        _camera.position = Vector3.Lerp(_camera.position, _targetPosition, _smoothSpeed * Time.deltaTime);
        _camera.LookAt(_player.position + Vector3.up * _cameraVerticalRotationOffset);
    }
}