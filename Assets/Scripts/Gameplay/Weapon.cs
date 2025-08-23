using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private int _maxAmmoPerStore;

    private int _currentAmmo;

    private void Awake()
    {
        _currentAmmo = _maxAmmoPerStore;
        SetAmmoText();
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
            return;

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    public void OnShootButton()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        SetAmmoText();
    }

    private void SetAmmoText()
    { 
        _ammoText.text = _currentAmmo + "/" + _maxAmmoPerStore;
    }
}