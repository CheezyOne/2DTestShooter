using UnityEngine;

public interface IRotationService
{
    Quaternion GetRotation(Vector3 movementDirection, Vector3 playerPosition);

}

public class DesktopRotationService : IRotationService
{
    private Camera _camera;
    private float _mouseSensitivity = 2f;
    private float _rotationY = 0f;
    private bool _isInitialized = false;

    public DesktopRotationService(Camera camera)
    {
        _camera = camera;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Quaternion GetRotation(Vector3 movementDirection, Vector3 playerPosition)
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;

        if (!_isInitialized)
        {
            _rotationY = _camera.transform.eulerAngles.y;
            _isInitialized = true;
        }

        _rotationY += mouseX;
        _rotationY %= 360f;
        return Quaternion.Euler(0f, _rotationY, 0f);
    }
}

public class MobileRotationService : IRotationService
{
    public Quaternion GetRotation(Vector3 movementDirection, Vector3 playerPosition)
    {
        if (movementDirection != Vector3.zero)
        {
            return Quaternion.LookRotation(movementDirection);
        }
        return Quaternion.identity;
    }
}