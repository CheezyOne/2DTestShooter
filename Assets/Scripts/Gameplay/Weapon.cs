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
    [SerializeField] private SoundType _shootSound;
    [SerializeField] private LineRenderer _laserSight;
    [SerializeField] private float _laserMaxDistance;
    [SerializeField] private LayerMask _laserHitLayers;

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

    public void EnableLaserSight(bool enable)
    {
        if (_laserSight != null)
        {
            _laserSight.enabled = enable;
        }
    }

    private void UpdateLaserSight()
    {
        Vector3 laserDirection = _bulletSpawnPoint.forward;
        Vector3 startPoint = _bulletSpawnPoint.position;
        Vector3 endPoint;
        RaycastHit hit;

        if (Physics.Raycast(startPoint, laserDirection, out hit, _laserMaxDistance, _laserHitLayers))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = startPoint + laserDirection * _laserMaxDistance;
        }

        _laserSight.SetPosition(0, startPoint);
        _laserSight.SetPosition(1, endPoint);
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
        SoundsManager.Instance.PlaySound(_shootSound);

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
        if(_laserSight!=null)
            UpdateLaserSight();

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

    private void OnEnable()
    {
        EnableLaserSight(true);
    }

    private void OnDisable()
    {
        EnableLaserSight(false);
        _reloadImage.fillAmount = 0;
        _reloadTimePassed = 0;
    }
}