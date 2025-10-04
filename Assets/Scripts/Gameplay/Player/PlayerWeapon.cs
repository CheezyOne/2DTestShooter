using TMPro;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private Weapon[] _weapons;

    private Weapon _currentWeapon;
    private int _weaponIndex = 0;
    private bool _isShootingDisabled;

    private void Awake()
    {
        SetCurrentWeapon();
    }

    private void Shoot()
    {
        _currentWeapon.TryShoot();
        SetAmmoText();
    }

    public void OnShootButton()
    {
        Shoot();
    }

    public void OnChangeWeaponButton()
    {
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        _currentWeapon.gameObject.SetActive(false);
        _weaponIndex++;

        if (_weaponIndex >= _weapons.Length)
            _weaponIndex = 0;

        SetCurrentWeapon();
    }

    private void SetCurrentWeapon()
    {
        _currentWeapon = _weapons[_weaponIndex];
        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.Init();
        SetAmmoText();
    }

    private void Update()
    {
        if (_isShootingDisabled)
            return;

        if (Application.isMobilePlatform)
            return;

        if (Input.GetMouseButtonDown(0))
            Shoot();

        if (Input.GetKeyDown(KeyCode.Q))
            ChangeWeapon();
    }

    private void SetAmmoText()
    {
        _ammoText.text = _currentWeapon.CurrentAmmo + "/" + _currentWeapon.MaxAmmoPerStore;
    }

    private void DisableShooting()
    {
        _isShootingDisabled = true;
    }

    private void EnableShooting()
    {
        _isShootingDisabled = false;
    }

    private void OnEnable()
    {
        EventBus.OnUpgradeWindowClose += EnableShooting;
        EventBus.OnUpgradeWindowOpen += DisableShooting;
        EventBus.OnReloadComplete += SetAmmoText;
    }

    private void OnDisable()
    {
        EventBus.OnUpgradeWindowClose -= EnableShooting;
        EventBus.OnUpgradeWindowOpen -= DisableShooting;
        EventBus.OnReloadComplete -= SetAmmoText;
    }
}