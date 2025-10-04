using UnityEngine;
using System.Collections.Generic;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSpeedIncreaseStep;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask[] _obstaclesLayers;

    private Vector3 _movement;
    private IInputService _inputService;
    private IRotationService _rotationService;
    private LayerMask _combinedObstaclesMask;
    private List<ContactPoint> _collisionContacts = new List<ContactPoint>();
    private bool _isMovementDisabled;

    private const float MINIMAL_MAGNITUDE_THRESHOLD = 0.1f;

    public void Initialize(IInputService inputService, IRotationService rotationService)
    {
        _inputService = inputService;
        _rotationService = rotationService;
    }

    private void FixedUpdate()
    {
        if(_isMovementDisabled)
            return;
        
        HandleMovement();
        HandleRotation();
    }

    private void Awake()
    {
        UpdateCombinedObstaclesMask();
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
        Vector3 direction = _inputService.GetMovementDirection();
        _movement = new Vector3(direction.x, 0, direction.z).normalized;
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
            _rigidbody.MoveRotation(targetRotation);
        }
    }

    private void DisableMovements()
    {
        _isMovementDisabled = true;
    }

    private void EnableMovements()
    {
        _isMovementDisabled = false;
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