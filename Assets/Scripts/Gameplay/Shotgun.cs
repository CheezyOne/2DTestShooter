using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int _bulletCount;
    [SerializeField] private float _spreadAngle;

    protected override void Shoot()
    {
        float angleStep = (_spreadAngle * 2) / (_bulletCount - 1);
        float startAngle = -_spreadAngle;

        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 rotatedDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
            ShootBullet(rotatedDirection);
        }

        base.Shoot();
    }
}