using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _playerTransform;
    private IInputService _inputService;

    public void Initialize(IInputService inputService)
    {
        _inputService = inputService;
    }

    private void FixedUpdate()
    {
        Vector3 direction = _inputService.GetMovementDirection();
        _playerTransform.Translate(direction * _moveSpeed * Time.fixedDeltaTime);
    }
}