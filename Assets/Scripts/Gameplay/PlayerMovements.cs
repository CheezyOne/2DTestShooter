using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _movement;
    private IInputService _inputService;
    private IRotationService _rotationService;

    public void Initialize(IInputService inputService, IRotationService rotationService)
    {
        _inputService = inputService;
        _rotationService = rotationService;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 direction = _inputService.GetMovementDirection();
        _movement = new Vector3(direction.x, 0, direction.z);

        if (_movement.magnitude > 1)
            _movement.Normalize();

        Vector3 targetVelocity = _movement * _moveSpeed;
        Vector3 velocityChange = targetVelocity - _rigidbody.velocity;
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void HandleRotation()
    {
        if (_movement != Vector3.zero || _rotationService is DesktopRotationService)
        {
            Quaternion targetRotation = _rotationService.GetRotation(_movement, _playerTransform.position);
            _rigidbody.MoveRotation(targetRotation);
        }
    }
}