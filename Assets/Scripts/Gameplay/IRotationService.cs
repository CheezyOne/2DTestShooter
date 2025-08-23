using UnityEngine;

public interface IRotationService
{
    Quaternion GetRotation(Vector3 movementDirection, Vector3 playerPosition);

}

public class DesktopRotationService : IRotationService
{
    private Camera _camera;

    public DesktopRotationService(Camera camera)
    {
        _camera = camera;
    }

    public Quaternion GetRotation(Vector3 movementDirection, Vector3 playerPosition)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 direction = point - playerPosition;
            direction.y = 0; 

            if (direction != Vector3.zero)
            {
                return Quaternion.LookRotation(direction);
            }
        }

        return Quaternion.identity;
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