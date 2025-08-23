public class OneBulletWeapon : Weapon
{
    protected override void Shoot()
    {
        ShootBullet(transform.forward);
        base.Shoot();
    }
}