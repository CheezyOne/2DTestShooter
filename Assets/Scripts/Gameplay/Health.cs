using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamagable, IKillable
{
    [SerializeField] private Image _healthBarForeground;

    [SerializeField] protected float _maxHealth;

    protected float _currentHealth;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }

        _healthBarForeground.fillAmount = _currentHealth / _maxHealth;
    }

    public virtual void Die()
    {

    }
}