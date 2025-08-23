using UnityEngine;

public interface IInputService
{
    Vector3 GetMovementDirection();
}

public class DesktopInputService : IInputService
{
    public Vector3 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontal, 0, vertical).normalized;
    }
}

public class MobileInputService : IInputService
{
    private Joystick _joystick;

    public MobileInputService(Joystick joystick)
    {
        _joystick = joystick;
    }

    public Vector3 GetMovementDirection()
    {
        return new Vector3(
            _joystick.Horizontal,
            0,
            _joystick.Vertical
        ).normalized;
    }
}