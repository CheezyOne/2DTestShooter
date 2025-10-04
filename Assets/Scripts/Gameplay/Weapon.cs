using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int _maxAmmoPerStore;
    [SerializeField] private float _reloadingTime;
    [SerializeField] protected Transform _bulletSpawnPoint;
    [SerializeField] protected Bullet _bullet;
    [SerializeField] protected float _damage;
    [SerializeField] private Image _reloadImage;

    private bool _startingAmmoSet;
    private bool _isReloading;
    private int _currentAmmo;
    private float _reloadTimePassed;

    public int MaxAmmoPerStore => _maxAmmoPerStore;
    public int CurrentAmmo => _currentAmmo;

    public void Init()
    {
        if (_startingAmmoSet)
            return;

        _currentAmmo = _maxAmmoPerStore;
        _startingAmmoSet = true;
    }

    public void TryShoot()
    {
        if (_isReloading)
            return;

        Shoot();
    }

    protected virtual void Shoot()
    {
        _currentAmmo--;

        if (_currentAmmo <= 0)
            _isReloading = true;
    }

    protected void ShootBullet(Vector3 direction)
    {
        Bullet newBullet = PoolManager.Instance.InstantiateObject(_bullet, _bulletSpawnPoint.position, transform.rotation);
        newBullet.SetDamage(_damage);
        newBullet.Rigidbody.AddForce(direction * _bullet.BulletSpeed);
    }

    private void Update()
    {
        if (!_isReloading)
            return;

        _reloadTimePassed += Time.deltaTime;
        _reloadImage.fillAmount = _reloadTimePassed / _reloadingTime;

        if (_reloadTimePassed >= _reloadingTime)
        {
            _reloadImage.fillAmount = 0;
            _reloadTimePassed = 0;
            _isReloading = false;
            _currentAmmo = _maxAmmoPerStore;
            EventBus.OnReloadComplete?.Invoke();
        }
    }

    private void OnDisable()
    {
        _reloadImage.fillAmount = 0;
        _reloadTimePassed = 0;
    }
}