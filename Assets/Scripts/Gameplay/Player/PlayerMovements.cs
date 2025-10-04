using UnityEngine;
using System.Collections.Generic;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSpeedIncreaseStep;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask[] _obstaclesLayers;
    [SerializeField] private Camera _playerCamera;

    private Vector3 _movement;
    private IInputService _inputService;
    private IRotationService _rotationService;
    private LayerMask _combinedObstaclesMask;
    private List<ContactPoint> _collisionContacts = new List<ContactPoint>();
    private bool _isMovementDisabled;

    private Vector3 _cachedCameraForward;
    private Vector3 _cachedCameraRight;
    private float _cameraUpdateTimer;
    private const float CAMERA_UPDATE_INTERVAL = 0.1f;

    private const float MINIMAL_MAGNITUDE_THRESHOLD = 0.1f;

    public void Initialize(IInputService inputService, IRotationService rotationService)
    {
        _inputService = inputService;
        _rotationService = rotationService;
    }

    private void FixedUpdate()
    {
        if (_isMovementDisabled)
            return;

        UpdateCameraDirections();
        HandleMovement();
        HandleRotation();
    }

    private void Awake()
    {
        UpdateCombinedObstaclesMask();
        UpdateCameraDirectionsImmediate();
    }

    private void UpdateCameraDirections()
    {
        _cameraUpdateTimer += Time.fixedDeltaTime;

        if (_cameraUpdateTimer >= CAMERA_UPDATE_INTERVAL)
        {
            UpdateCameraDirectionsImmediate();
            _cameraUpdateTimer = 0f;
        }
    }

    private void UpdateCameraDirectionsImmediate()
    {
        _cachedCameraForward = _playerCamera.transform.forward;
        _cachedCameraForward.y = 0;
        _cachedCameraForward.Normalize();

        _cachedCameraRight = _playerCamera.transform.right;
        _cachedCameraRight.y = 0;
        _cachedCameraRight.Normalize();
    }

    private void UpdateCombinedObstaclesMask()
    {
        _combinedObstaclesMask = 0;

        foreach (LayerMask layer in _obstaclesLayers)
        {
            _combinedObstaclesMask |= layer;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _combinedObstaclesMask) != 0)
        {
            _collisionContacts.AddRange(collision.contacts);
        }
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = _inputService.GetMovementDirection();
        Vector3 cameraRelativeDirection = _cachedCameraForward * inputDirection.z + _cachedCameraRight * inputDirection.x;
        _movement = cameraRelativeDirection.normalized;
        Vector3 targetVelocity = _movement * _moveSpeed;

        if (_collisionContacts.Count > 0)
        {
            targetVelocity = GetAdjustedVelocity(targetVelocity);
            _collisionContacts.Clear();
        }

        Vector3 velocityChange = targetVelocity - _rigidbody.velocity;
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private Vector3 GetAdjustedVelocity(Vector3 desiredVelocity)
    {
        Vector3 adjustedVelocity = desiredVelocity;

        foreach (ContactPoint contact in _collisionContacts)
        {
            Vector3 collisionNormal = contact.normal;

            if (Vector3.Dot(desiredVelocity, collisionNormal) < 0)
            {
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(desiredVelocity, collisionNormal);
                projectedVelocity.y = 0;

                if (projectedVelocity.magnitude > MINIMAL_MAGNITUDE_THRESHOLD)
                {
                    adjustedVelocity = projectedVelocity.normalized * desiredVelocity.magnitude;
                }
                else
                {
                    adjustedVelocity = Vector3.zero;
                }
            }
        }

        return adjustedVelocity;
    }

    private void HandleRotation()
    {
        if (_movement != Vector3.zero || _rotationService is DesktopRotationService)
        {
            Quaternion targetRotation = _rotationService.GetRotation(_movement, _playerTransform.position);

            Quaternion smoothedRotation = Quaternion.Slerp(
                _rigidbody.rotation,
                targetRotation,
                Time.fixedDeltaTime * _rotationSpeed
            );

            _rigidbody.MoveRotation(smoothedRotation);
        }
    }

    private void DisableMovements()
    {
        _isMovementDisabled = true;

        if (_rotationService is DesktopRotationService)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void EnableMovements()
    {
        _isMovementDisabled = false;

        if(_rotationService is DesktopRotationService)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void IncreaseMoveSpeed()
    {
        _moveSpeed += _moveSpeedIncreaseStep;
    }

    private void OnEnable()
    {
        EventBus.OnUpgradeWindowClose += EnableMovements;
        EventBus.OnUpgradeWindowOpen += DisableMovements;
        EventBus.OnPlayerSpeedIncreased += IncreaseMoveSpeed;
    }

    private void OnDisable()
    {
        EventBus.OnUpgradeWindowClose -= EnableMovements;
        EventBus.OnUpgradeWindowOpen -= DisableMovements;
        EventBus.OnPlayerSpeedIncreased -= IncreaseMoveSpeed;
    }
}