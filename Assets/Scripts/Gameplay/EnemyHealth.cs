using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private DeathEffect _deathEffect;

    public override void Die()
    {
        PoolManager.Instance.InstantiateObject(_deathEffect, transform.position, Quaternion.identity);
        PoolManager.Instance.DestroyObject(gameObject);
        EventBus.OnEnemyDie?.Invoke();
    }

    private void OnDisable()
    {
        _currentHealth = _maxHealth;
    }
}