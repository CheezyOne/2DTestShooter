using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _maxHPIncrease;

    public override void Die()
    {
        EventBus.OnPlayerDie?.Invoke();
        Destroy(_player);
    }

    private void HealFullHP()
    {
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    private void IncreaseMaxHP()
    {
        _maxHealth += _maxHPIncrease;
        _currentHealth += _maxHPIncrease;
        UpdateHealth();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.OnPlayerHealed += HealFullHP;
        EventBus.OnPlayerMaxHPIncreased += IncreaseMaxHP;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerHealed -= HealFullHP;
        EventBus.OnPlayerMaxHPIncreased -= IncreaseMaxHP;
    }
}